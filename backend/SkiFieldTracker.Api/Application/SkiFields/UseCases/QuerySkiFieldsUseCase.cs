using System.Linq;
using SkiFieldTracker.Application.Abstractions.Repositories;
using SkiFieldTracker.Application.Common;
using SkiFieldTracker.Application.SkiFields.Extensions;
using SkiFieldTracker.Application.SkiFields.Models;

namespace SkiFieldTracker.Application.SkiFields.UseCases;

public interface IQuerySkiFieldsUseCase
{
    Task<PaginatedResult<SkiFieldResponse>> ExecuteAsync(FindManyRequest request, CancellationToken cancellationToken);
}

public sealed class QuerySkiFieldsUseCase(ISkiFieldRepository repository) : IQuerySkiFieldsUseCase
{
    public async Task<PaginatedResult<SkiFieldResponse>> ExecuteAsync(FindManyRequest request, CancellationToken cancellationToken)
    {
        var normalizedRequest = NormalizeRequest(request);
        var totalCount = await repository.CountAsync(normalizedRequest.Where, cancellationToken);
        var entities = await repository.QueryAsync(normalizedRequest, cancellationToken);
        var responses = entities.Select(x => x.ToResponse()).ToList();
        return new PaginatedResult<SkiFieldResponse>
        {
            Items = responses,
            TotalCount = totalCount
        };
    }

    private static FindManyRequest NormalizeRequest(FindManyRequest request)
    {
        var skip = request.Skip is null or < 0 ? 0 : request.Skip;
        var take = request.Take is null or <= 0 ? 10 : request.Take;

        return new FindManyRequest
        {
            Where = request.Where,
            OrderBy = request.OrderBy is { Count: > 0 } ? request.OrderBy : new[] { new OrderByClause { Field = "name", Direction = SortDirection.Asc } },
            Skip = skip,
            Take = take
        };
    }
}

