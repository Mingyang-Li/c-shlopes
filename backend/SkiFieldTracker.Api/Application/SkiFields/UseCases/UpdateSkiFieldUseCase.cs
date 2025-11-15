using SkiFieldTracker.Application.Abstractions.Repositories;
using SkiFieldTracker.Application.SkiFields.Extensions;
using SkiFieldTracker.Application.SkiFields.Models;

namespace SkiFieldTracker.Application.SkiFields.UseCases;

public interface IUpdateSkiFieldUseCase
{
    Task<SkiFieldResponse> ExecuteAsync(Guid id, UpdateSkiFieldRequest request, CancellationToken cancellationToken);
}

public sealed class UpdateSkiFieldUseCase(ISkiFieldRepository repository) : IUpdateSkiFieldUseCase
{
    public async Task<SkiFieldResponse> ExecuteAsync(Guid id, UpdateSkiFieldRequest request, CancellationToken cancellationToken)
    {
        var skiField = await repository.GetByIdAsync(id, cancellationToken);

        if (skiField is null)
        {
            throw new SkiFieldNotFoundException(id);
        }

        var normalizedName = request.Name.Trim();
        if (!string.Equals(skiField.Name, normalizedName, StringComparison.OrdinalIgnoreCase) &&
            await repository.ExistsByNameAsync(normalizedName, cancellationToken))
        {
            throw new SkiFieldNameAlreadyExistsException(normalizedName);
        }

        skiField.Name = normalizedName;
        skiField.CountryCode = request.CountryCode.Trim().ToUpperInvariant();
        skiField.Region = request.Region.Trim();
        skiField.FullDayPassPrice = decimal.Round(request.FullDayPassPrice, 2, MidpointRounding.AwayFromZero);
        skiField.Currency = request.Currency.Trim().ToUpperInvariant();
        skiField.NearestTown = request.NearestTown.Trim();
        skiField.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(skiField, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return skiField.ToResponse();
    }
}

public sealed class SkiFieldNotFoundException(Guid id)
    : KeyNotFoundException($"Ski field '{id}' was not found.");

