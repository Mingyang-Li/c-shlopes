using SkiFieldTracker.Application.Abstractions.Repositories;

namespace SkiFieldTracker.Application.SkiFields.UseCases;

public interface IDeleteSkiFieldUseCase
{
    Task ExecuteAsync(Guid id, CancellationToken cancellationToken);
}

public sealed class DeleteSkiFieldUseCase(ISkiFieldRepository repository) : IDeleteSkiFieldUseCase
{
    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
        var skiField = await repository.GetByIdAsync(id, cancellationToken);

        if (skiField is null)
        {
            throw new SkiFieldNotFoundException(id);
        }

        await repository.DeleteAsync(skiField, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}

