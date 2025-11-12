using SkiFieldTracker.Application.SkiFields.Models;
using SkiFieldTracker.Domain.Entities;

namespace SkiFieldTracker.Application.SkiFields.Extensions;

public static class SkiFieldMappingExtensions
{
    public static SkiFieldResponse ToResponse(this SkiField entity) =>
        new(
            entity.Id,
            entity.Name,
            entity.Country,
            entity.Region,
            entity.AdultFullDayPassUsd,
            entity.CreatedAt,
            entity.UpdatedAt);
}

