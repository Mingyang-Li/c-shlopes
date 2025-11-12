using SkiFieldTracker.Application.Abstractions.Repositories;
using SkiFieldTracker.Application.SkiFields.Extensions;
using SkiFieldTracker.Application.SkiFields.Models;
using SkiFieldTracker.Domain.Entities;

namespace SkiFieldTracker.Application.SkiFields.UseCases;

public interface ICreateSkiFieldUseCase
{
    Task<SkiFieldResponse> ExecuteAsync(CreateSkiFieldRequest request, CancellationToken cancellationToken);
}

public sealed class CreateSkiFieldUseCase(ISkiFieldRepository repository) : ICreateSkiFieldUseCase
{
    public async Task<SkiFieldResponse> ExecuteAsync(CreateSkiFieldRequest request, CancellationToken cancellationToken)
    {
        var normalizedName = request.Name.Trim();

        if (await repository.ExistsByNameAsync(normalizedName, cancellationToken))
        {
            throw new SkiFieldNameAlreadyExistsException(normalizedName);
        }

        var now = DateTime.UtcNow;
        var entity = new SkiField
        {
            Id = Guid.NewGuid(),
            Name = normalizedName,
            Country = request.Country.Trim(),
            Region = request.Region.Trim(),
            AdultFullDayPassUsd = decimal.Round(request.AdultFullDayPassUsd, 2, MidpointRounding.AwayFromZero),
            CreatedAt = now,
            UpdatedAt = now
        };

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.ToResponse();
    }
}

public sealed class SkiFieldNameAlreadyExistsException(string name)
    : InvalidOperationException($"Ski field '{name}' already exists.");

