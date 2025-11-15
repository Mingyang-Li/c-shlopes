using SkiFieldTracker.Application.SkiFields.Models;
using SkiFieldTracker.Domain.Entities;

namespace SkiFieldTracker.Application.SkiFields.Extensions;

public static class SkiFieldMappingExtensions
{
    public static SkiFieldResponse ToResponse(this SkiField entity) =>
        new()
        {
            Id = entity.Uid, // Map Uid to Id in response
            Name = entity.Name,
            CountryCode = entity.CountryCode,
            Region = entity.Region,
            FullDayPassPrice = entity.FullDayPassPrice,
            Currency = entity.Currency,
            NearestTown = entity.NearestTown,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
}

