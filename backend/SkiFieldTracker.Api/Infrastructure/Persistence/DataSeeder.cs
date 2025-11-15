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

    public async Task CleanAsync(CancellationToken cancellationToken)
    {
        var count = await _dbContext.SkiFields.CountAsync(cancellationToken);
        if (count == 0)
        {
            _logger.LogInformation("Database is already empty. Nothing to clean.");
            return;
        }

        _logger.LogInformation("Cleaning {Count} ski fields from database...", count);
        _dbContext.SkiFields.RemoveRange(_dbContext.SkiFields);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Database cleaned successfully.");
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var seedItems = SeedData.GetSkiFields();

        if (seedItems.Count == 0)
        {
            _logger.LogInformation("No seed data found.");
            return;
        }

        var existingNames = await _dbContext.SkiFields
            .Select(x => x.Name.ToLower())
            .ToListAsync(cancellationToken);

        var newEntities = seedItems
            .Where(item => !existingNames.Contains(item.Name.Trim().ToLower()))
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
}

