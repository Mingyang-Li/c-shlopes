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

public sealed class SkiFieldResponse
{
    public string Id { get; init; } = string.Empty; // This is the Uid, exposed as "id" to clients
    public string Name { get; init; } = string.Empty;
    public string CountryCode { get; init; } = string.Empty;
    public string Region { get; init; } = string.Empty;
    public decimal FullDayPassPrice { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string NearestTown { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

