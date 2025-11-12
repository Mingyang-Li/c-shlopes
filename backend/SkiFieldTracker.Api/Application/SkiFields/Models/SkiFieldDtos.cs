using System.ComponentModel.DataAnnotations;

namespace SkiFieldTracker.Application.SkiFields.Models;

public sealed class CreateSkiFieldRequest
{
    [Required]
    [MaxLength(120)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Country { get; init; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Region { get; init; } = string.Empty;

    [Range(0, 10000)]
    public decimal AdultFullDayPassUsd { get; init; }
}

public sealed class UpdateSkiFieldRequest
{
    [Required]
    [MaxLength(120)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Country { get; init; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Region { get; init; } = string.Empty;

    [Range(0, 10000)]
    public decimal AdultFullDayPassUsd { get; init; }
}

public sealed record SkiFieldResponse(
    Guid Id,
    string Name,
    string Country,
    string Region,
    decimal AdultFullDayPassUsd,
    DateTime CreatedAt,
    DateTime UpdatedAt);

