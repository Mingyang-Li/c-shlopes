using SkiFieldTracker.Domain.Entities;
using SkiFieldTracker.Application.Common;
using SkiFieldTracker.Application.SkiFields.Models;

namespace SkiFieldTracker.Application.Abstractions.Repositories;

public interface ISkiFieldRepository
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
    Task<SkiField?> GetByUidAsync(string uid, CancellationToken cancellationToken);
    Task AddAsync(SkiField skiField, CancellationToken cancellationToken);
    Task UpdateAsync(SkiField skiField, CancellationToken cancellationToken);
    Task DeleteAsync(SkiField skiField, CancellationToken cancellationToken);
    Task<int> CountAsync(SkiFieldFilter? filter, CancellationToken cancellationToken);
    Task<IReadOnlyList<SkiField>> QueryAsync(FindManyRequest request, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}

