using SkiFieldTracker.Application.Abstractions.Repositories;

namespace SkiFieldTracker.Application.SkiFields.UseCases;

public interface IDeleteSkiFieldUseCase
{
    Task ExecuteAsync(string uid, CancellationToken cancellationToken);
}

public sealed class DeleteSkiFieldUseCase(ISkiFieldRepository repository) : IDeleteSkiFieldUseCase
{
    public async Task ExecuteAsync(string uid, CancellationToken cancellationToken)
    {
        var skiField = await repository.GetByUidAsync(uid, cancellationToken);

        if (skiField is null)
        {
            throw new SkiFieldNotFoundException(uid);
        }

        await repository.DeleteAsync(skiField, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}

