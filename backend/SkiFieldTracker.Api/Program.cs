using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using SkiFieldTracker.Application.Abstractions.Repositories;
using SkiFieldTracker.Application.SkiFields.UseCases;
using SkiFieldTracker.Infrastructure.Persistence;
using SkiFieldTracker.Infrastructure.Repositories;

var seedOnly = args.Any(a => string.Equals(a, "--seed", StringComparison.OrdinalIgnoreCase));
var cleanOnly = args.Any(a => string.Equals(a, "--clean", StringComparison.OrdinalIgnoreCase));
var resetDb = args.Any(a => string.Equals(a, "--reset", StringComparison.OrdinalIgnoreCase));

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var problemDetails = new ValidationProblemDetails(context.ModelState)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred."
        };
        return new BadRequestObjectResult(problemDetails);
    };
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ski Field Tracker API",
        Version = "v1",
        Description = "REST API letting you explore ski field options around the world"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured. Please set it in appsettings.json or environment variables.");

var allowedCorsOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

var logger = LoggerFactory.Create(config => config.AddConsole()).CreateLogger("CorsConfig");
logger.LogInformation("Environment: {Environment}", builder.Environment.EnvironmentName);
logger.LogInformation("Allowed CORS origins count: {Count}", allowedCorsOrigins.Length);
if (allowedCorsOrigins.Length > 0)
{
    logger.LogInformation("Allowed origins: {Origins}", string.Join(", ", allowedCorsOrigins));
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        // allow all origins for now
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Force IPv4 by resolving hostname to IPv4 address
    var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
    if (!string.IsNullOrEmpty(connectionStringBuilder.Host) && 
        !IPAddress.TryParse(connectionStringBuilder.Host, out _))
    {
        try
        {
            var hostEntry = Dns.GetHostEntry(connectionStringBuilder.Host);
            var ipv4Address = hostEntry.AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            
            if (ipv4Address != null)
            {
                connectionStringBuilder.Host = ipv4Address.ToString();
                connectionString = connectionStringBuilder.ToString();
            }
        }
        catch
        {
            // If DNS resolution fails, continue with original hostname
        }
    }
    
    options.UseNpgsql(connectionString);
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddScoped<DataSeeder>();
builder.Services.AddScoped<ISkiFieldRepository, SkiFieldRepository>();
builder.Services.AddScoped<ICreateSkiFieldUseCase, CreateSkiFieldUseCase>();
builder.Services.AddScoped<IUpdateSkiFieldUseCase, UpdateSkiFieldUseCase>();
builder.Services.AddScoped<IDeleteSkiFieldUseCase, DeleteSkiFieldUseCase>();
builder.Services.AddScoped<IQuerySkiFieldsUseCase, QuerySkiFieldsUseCase>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.EnsureDatabaseMigratedAsync(CancellationToken.None);

    if (cleanOnly)
    {
        await seeder.CleanAsync(CancellationToken.None);
        return;
    }

    if (resetDb)
    {
        await seeder.CleanAsync(CancellationToken.None);
        await seeder.SeedAsync(CancellationToken.None);
        return;
    }

    await seeder.SeedAsync(CancellationToken.None);
}

if (seedOnly || cleanOnly || resetDb)
{
    return;
}

// CORS must be before other middleware
app.UseCors("DefaultCors");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ski Field Tracker API v1");
        options.DisplayRequestDuration();
    });

    // Disable HTTPS redirection in development to avoid CORS issues
    // app.UseHttpsRedirection();
}
else
{
    app.UseHttpsRedirection();
}

app.MapControllers();

await app.RunAsync();

