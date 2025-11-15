namespace SkiFieldTracker.Domain.Entities;

public class SkiField
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public decimal FullDayPassPrice { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string NearestTown { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

