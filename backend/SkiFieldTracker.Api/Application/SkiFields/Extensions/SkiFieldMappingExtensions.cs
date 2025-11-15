using SkiFieldTracker.Application.SkiFields.Models;
using SkiFieldTracker.Domain.Entities;

namespace SkiFieldTracker.Application.SkiFields.Extensions;

public static class SkiFieldMappingExtensions
{
    public static SkiFieldResponse ToResponse(this SkiField entity) =>
        new(
            entity.Id,
            entity.Name,
            entity.CountryCode,
            entity.Region,
            entity.FullDayPassPrice,
            entity.Currency,
            entity.NearestTown,
            entity.CreatedAt,
            entity.UpdatedAt);
}

