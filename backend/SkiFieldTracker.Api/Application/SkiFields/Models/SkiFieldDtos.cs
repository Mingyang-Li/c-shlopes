using System.ComponentModel.DataAnnotations;

namespace SkiFieldTracker.Application.SkiFields.Models;

public sealed class CreateSkiFieldRequest
{
    [Required]
    [MaxLength(120)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [MaxLength(3)]
    public string CountryCode { get; init; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Region { get; init; } = string.Empty;

    [Range(0, 10000)]
    public decimal FullDayPassPrice { get; init; }

    [Required]
    [MaxLength(3)]
    public string Currency { get; init; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string NearestTown { get; init; } = string.Empty;
}

public sealed class UpdateSkiFieldRequest
{
    [Required]
    [MaxLength(120)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [MaxLength(3)]
    public string CountryCode { get; init; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Region { get; init; } = string.Empty;

    [Range(0, 10000)]
    public decimal FullDayPassPrice { get; init; }

    [Required]
    [MaxLength(3)]
    public string Currency { get; init; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string NearestTown { get; init; } = string.Empty;
}

public sealed record SkiFieldResponse(
    Guid Id,
    string Name,
    string CountryCode,
    string Region,
    decimal FullDayPassPrice,
    string Currency,
    string NearestTown,
    DateTime CreatedAt,
    DateTime UpdatedAt);

