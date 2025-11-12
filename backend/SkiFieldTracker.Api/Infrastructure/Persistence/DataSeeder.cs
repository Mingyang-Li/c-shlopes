using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkiFieldTracker.Domain.Entities;

namespace SkiFieldTracker.Infrastructure.Persistence;

public class DataSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(ApplicationDbContext dbContext, ILogger<DataSeeder> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task EnsureDatabaseMigratedAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Applying pending EF Core migrations (if any)...");
        await _dbContext.Database.MigrateAsync(cancellationToken);
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var seedFilePath = Path.Combine(AppContext.BaseDirectory, "Seeds", "ski-fields.json");
        if (!File.Exists(seedFilePath))
        {
            _logger.LogWarning("Seed file {SeedFile} not found. Skipping seeding.", seedFilePath);
            return;
        }

        var json = await File.ReadAllTextAsync(seedFilePath, cancellationToken);

        var seedItems = JsonSerializer.Deserialize<List<SeedSkiField>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? [];

        if (seedItems.Count == 0)
        {
            _logger.LogInformation("No seed data found in {SeedFile}.", seedFilePath);
            return;
        }

        var existingNames = await _dbContext.SkiFields
            .Select(x => x.Name.ToLower())
            .ToListAsync(cancellationToken);

        var now = DateTime.UtcNow;
        var newEntities = seedItems
            .Where(item => !existingNames.Contains(item.Name.Trim().ToLower()))
            .Select(item => new SkiField
            {
                Id = Guid.NewGuid(),
                Name = item.Name.Trim(),
                Country = item.Country.Trim(),
                Region = item.Region.Trim(),
                AdultFullDayPassUsd = decimal.Round(item.AdultFullDayPassUsd, 2, MidpointRounding.AwayFromZero),
                CreatedAt = now,
                UpdatedAt = now
            })
            .ToList();

        if (newEntities.Count == 0)
        {
            _logger.LogInformation("Seed data already present. No new ski fields inserted.");
            return;
        }

        _logger.LogInformation("Seeding {Count} ski fields.", newEntities.Count);
        await _dbContext.SkiFields.AddRangeAsync(newEntities, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private sealed record SeedSkiField(
        string Name,
        string Country,
        string Region,
        decimal AdultFullDayPassUsd);
}

