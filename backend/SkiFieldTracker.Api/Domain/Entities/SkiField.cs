namespace SkiFieldTracker.Domain.Entities;

public class SkiField
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public decimal AdultFullDayPassUsd { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

