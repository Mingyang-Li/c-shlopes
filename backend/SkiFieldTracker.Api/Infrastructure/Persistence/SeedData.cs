using SkiFieldTracker.Domain.Entities;

namespace SkiFieldTracker.Infrastructure.Persistence;

public static class SeedData
{
    public static IReadOnlyList<SkiField> GetSkiFields()
    {
        var now = DateTime.UtcNow;
        return new List<SkiField>
        {
            // Canada
            new() { Id = Guid.NewGuid(), Name = "Whistler Blackcomb", CountryCode = "CAN", Region = "British Columbia", FullDayPassPrice = 185.00m, Currency = "CAD", NearestTown = "Whistler", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "Lake Louise", CountryCode = "CAN", Region = "Alberta", FullDayPassPrice = 135.00m, Currency = "CAD", NearestTown = "Lake Louise", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "Banff Sunshine", CountryCode = "CAN", Region = "Alberta", FullDayPassPrice = 130.00m, Currency = "CAD", NearestTown = "Banff", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "Revelstoke", CountryCode = "CAN", Region = "British Columbia", FullDayPassPrice = 125.00m, Currency = "CAD", NearestTown = "Revelstoke", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "Cypress Mountain", CountryCode = "CAN", Region = "British Columbia", FullDayPassPrice = 89.00m, Currency = "CAD", NearestTown = "Vancouver", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "Marmot Basin", CountryCode = "CAN", Region = "Alberta", FullDayPassPrice = 105.00m, Currency = "CAD", NearestTown = "Jasper", CreatedAt = now, UpdatedAt = now },

            // United States
            new() { Id = Guid.NewGuid(), Name = "Vail", CountryCode = "USA", Region = "Colorado", FullDayPassPrice = 199.00m, Currency = "USD", NearestTown = "Vail", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "Aspen Snowmass", CountryCode = "USA", Region = "Colorado", FullDayPassPrice = 209.00m, Currency = "USD", NearestTown = "Aspen", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "Beaver Creek", CountryCode = "USA", Region = "Colorado", FullDayPassPrice = 195.00m, Currency = "USD", NearestTown = "Avon", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "Jackson Hole", CountryCode = "USA", Region = "Wyoming", FullDayPassPrice = 189.00m, Currency = "USD", NearestTown = "Jackson", CreatedAt = now, UpdatedAt = now },

            // Japan
            new() { Id = Guid.NewGuid(), Name = "Niseko", CountryCode = "JPN", Region = "Hokkaido", FullDayPassPrice = 8500.00m, Currency = "JPY", NearestTown = "Niseko", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "Hakuba", CountryCode = "JPN", Region = "Nagano", FullDayPassPrice = 6500.00m, Currency = "JPY", NearestTown = "Hakuba", CreatedAt = now, UpdatedAt = now },

            // Australia
            new() { Id = Guid.NewGuid(), Name = "Perisher", CountryCode = "AUS", Region = "New South Wales", FullDayPassPrice = 165.00m, Currency = "AUD", NearestTown = "Jindabyne", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "Mt Buller", CountryCode = "AUS", Region = "Victoria", FullDayPassPrice = 155.00m, Currency = "AUD", NearestTown = "Mansfield", CreatedAt = now, UpdatedAt = now },

            // New Zealand
            new() { Id = Guid.NewGuid(), Name = "Coronet Peak", CountryCode = "NZL", Region = "Otago", FullDayPassPrice = 135.00m, Currency = "NZD", NearestTown = "Queenstown", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "The Remarkables", CountryCode = "NZL", Region = "Otago", FullDayPassPrice = 135.00m, Currency = "NZD", NearestTown = "Queenstown", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "Roundhill", CountryCode = "NZL", Region = "Canterbury", FullDayPassPrice = 95.00m, Currency = "NZD", NearestTown = "Lake Tekapo", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "Ohau", CountryCode = "NZL", Region = "Canterbury", FullDayPassPrice = 89.00m, Currency = "NZD", NearestTown = "Twizel", CreatedAt = now, UpdatedAt = now },

            // France
            new() { Id = Guid.NewGuid(), Name = "Chamonix", CountryCode = "FRA", Region = "Auvergne-Rhône-Alpes", FullDayPassPrice = 65.00m, Currency = "EUR", NearestTown = "Chamonix", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "Courchevel", CountryCode = "FRA", Region = "Auvergne-Rhône-Alpes", FullDayPassPrice = 68.00m, Currency = "EUR", NearestTown = "Courchevel", CreatedAt = now, UpdatedAt = now },

            // Switzerland
            new() { Id = Guid.NewGuid(), Name = "Zermatt", CountryCode = "CHE", Region = "Valais", FullDayPassPrice = 88.00m, Currency = "CHF", NearestTown = "Zermatt", CreatedAt = now, UpdatedAt = now },

            // Austria
            new() { Id = Guid.NewGuid(), Name = "St. Anton", CountryCode = "AUT", Region = "Tyrol", FullDayPassPrice = 62.00m, Currency = "EUR", NearestTown = "St. Anton", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), Name = "Kitzbühel", CountryCode = "AUT", Region = "Tyrol", FullDayPassPrice = 60.00m, Currency = "EUR", NearestTown = "Kitzbühel", CreatedAt = now, UpdatedAt = now },

            // Italy
            new() { Id = Guid.NewGuid(), Name = "Cortina d'Ampezzo", CountryCode = "ITA", Region = "Veneto", FullDayPassPrice = 58.00m, Currency = "EUR", NearestTown = "Cortina d'Ampezzo", CreatedAt = now, UpdatedAt = now },

            // Chile
            new() { Id = Guid.NewGuid(), Name = "Portillo", CountryCode = "CHL", Region = "Valparaíso", FullDayPassPrice = 85.00m, Currency = "USD", NearestTown = "Los Andes", CreatedAt = now, UpdatedAt = now }
        };
    }
}

