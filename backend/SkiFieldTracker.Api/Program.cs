using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using SkiFieldTracker.Application.Abstractions.Repositories;
using SkiFieldTracker.Application.SkiFields.UseCases;
using SkiFieldTracker.Infrastructure.Persistence;
using SkiFieldTracker.Infrastructure.Repositories;

var seedOnly = args.Any(a => string.Equals(a, "--seed", StringComparison.OrdinalIgnoreCase));

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
        Description = "REST API for managing ski field information with Prisma-style filtering semantics."
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Host=localhost;Port=5432;Database=ski_field_tracker;Username=ski_admin;Password=ski_admin_pw";

var allowedCorsOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        if (allowedCorsOrigins.Length == 0)
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        else
        {
            policy.WithOrigins(allowedCorsOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
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
    await seeder.SeedAsync(CancellationToken.None);
}

if (seedOnly)
{
    return;
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ski Field Tracker API v1");
        options.DisplayRequestDuration();
    });
}

app.UseCors("DefaultCors");

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();

