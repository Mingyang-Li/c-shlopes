using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SkiFieldTracker.Infrastructure.Persistence;

var options = SeedCommandOptions.Parse(args);
if (!options.HasActions)
{
    Console.WriteLine("Specify at least one action: --migrate, --seed, --clean or --reset.");
    return 1;
}

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddLogging(logging =>
{
    logging.AddSimpleConsole(console =>
    {
        console.IncludeScopes = false;
        console.SingleLine = true;
        console.TimestampFormat = "HH:mm:ss ";
    });
});

var apiProjectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "SkiFieldTracker.Api"));
builder.Configuration
    .AddJsonFile(Path.Combine(apiProjectPath, "appsettings.json"), optional: true, reloadOnChange: false)
    .AddJsonFile(Path.Combine(apiProjectPath, $"appsettings.{builder.Environment.EnvironmentName}.json"), optional: true, reloadOnChange: false)
    .AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing. Set it via appsettings or environment variables.");

builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(connectionString);
    optionsBuilder.UseSnakeCaseNamingConvention();
});
builder.Services.AddScoped<DataSeeder>();

var host = builder.Build();

try
{
    await using var scope = host.Services.CreateAsyncScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Seeder");

    if (options.RunMigrations)
    {
        logger.LogInformation("Applying EF Core migrations...");
        await seeder.EnsureDatabaseMigratedAsync(CancellationToken.None);
    }

    if (options.CleanData)
    {
        logger.LogInformation("Cleaning database...");
        await seeder.CleanAsync(CancellationToken.None);
    }

    if (options.SeedData)
    {
        logger.LogInformation("Seeding database...");
        await seeder.SeedAsync(CancellationToken.None);
    }

    logger.LogInformation("Seeder completed.");
    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Seeder failed: {ex.Message}");
    Console.Error.WriteLine(ex);
    return 1;
}
finally
{
    await host.StopAsync();
}

internal readonly record struct SeedCommandOptions(bool RunMigrations, bool SeedData, bool CleanData)
{
    public bool HasActions => RunMigrations || SeedData || CleanData;

    public static SeedCommandOptions Parse(string[] args)
    {
        var runMigrations = args.Any(a => a.Equals("--migrate", StringComparison.OrdinalIgnoreCase));
        var seed = args.Any(a => a.Equals("--seed", StringComparison.OrdinalIgnoreCase));
        var clean = args.Any(a => a.Equals("--clean", StringComparison.OrdinalIgnoreCase));
        var reset = args.Any(a => a.Equals("--reset", StringComparison.OrdinalIgnoreCase));

        if (reset)
        {
            return new SeedCommandOptions(RunMigrations: true, SeedData: true, CleanData: true);
        }

        return new SeedCommandOptions(RunMigrations: runMigrations, SeedData: seed, CleanData: clean);
    }
}

