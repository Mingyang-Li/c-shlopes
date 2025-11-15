using Microsoft.EntityFrameworkCore;
using SkiFieldTracker.Application.Abstractions.Repositories;
using SkiFieldTracker.Application.SkiFields.Models;
using SkiFieldTracker.Domain.Entities;
using SkiFieldTracker.Infrastructure.Persistence;

namespace SkiFieldTracker.Infrastructure.Repositories;

public class SkiFieldRepository : ISkiFieldRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SkiFieldRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
    {
        var trimmed = name.Trim();
        return _dbContext.SkiFields.AnyAsync(
            x => EF.Functions.ILike(x.Name, trimmed),
            cancellationToken);
    }

    public Task<SkiField?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _dbContext.SkiFields.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(SkiField skiField, CancellationToken cancellationToken) =>
        await _dbContext.SkiFields.AddAsync(skiField, cancellationToken);

    public Task UpdateAsync(SkiField skiField, CancellationToken cancellationToken)
    {
        _dbContext.SkiFields.Update(skiField);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(SkiField skiField, CancellationToken cancellationToken)
    {
        _dbContext.SkiFields.Remove(skiField);
        return Task.CompletedTask;
    }

    public async Task<int> CountAsync(SkiFieldFilter? filter, CancellationToken cancellationToken)
    {
        var query = ApplyFilter(_dbContext.SkiFields.AsQueryable(), filter);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SkiField>> QueryAsync(FindManyRequest request, CancellationToken cancellationToken)
    {
        var query = ApplyFilter(_dbContext.SkiFields.AsQueryable(), request.Where);
        query = ApplyOrdering(query, request.OrderBy);

        if (request.Skip is > 0)
        {
            query = query.Skip(request.Skip.Value);
        }

        if (request.Take is > 0)
        {
            query = query.Take(request.Take.Value);
        }

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        _dbContext.SaveChangesAsync(cancellationToken);

    private IQueryable<SkiField> ApplyFilter(IQueryable<SkiField> query, SkiFieldFilter? filter)
    {
        if (filter is null)
        {
            return query;
        }

        if (filter.Name is { } nameFilter)
        {
            query = ApplyStringFilter(query, nameFilter, nameof(SkiField.Name));
        }

        if (filter.CountryCode is { } countryCodeFilter)
        {
            query = ApplyStringFilter(query, countryCodeFilter, nameof(SkiField.CountryCode));
        }

        if (filter.Region is { } regionFilter)
        {
            query = ApplyStringFilter(query, regionFilter, nameof(SkiField.Region));
        }

        if (filter.FullDayPassPrice is { } priceFilter)
        {
            query = ApplyNumericFilter(query, priceFilter, nameof(SkiField.FullDayPassPrice));
        }

        if (filter.Currency is { } currencyFilter)
        {
            query = ApplyStringFilter(query, currencyFilter, nameof(SkiField.Currency));
        }

        if (filter.NearestTown is { } nearestTownFilter)
        {
            query = ApplyStringFilter(query, nearestTownFilter, nameof(SkiField.NearestTown));
        }

        return query;
    }

    private static IQueryable<SkiField> ApplyStringFilter(
        IQueryable<SkiField> query,
        StringFilter filter,
        string propertyName)
    {
        if (!string.IsNullOrWhiteSpace(filter.EqualTo))
        {
            var value = filter.EqualTo.Trim();
            query = filter.Mode == StringFilterMode.Insensitive
                ? query.Where(x => EF.Functions.ILike(EF.Property<string>(x, propertyName), value))
                : query.Where(x => EF.Property<string>(x, propertyName) == value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Contains))
        {
            var pattern = $"%{filter.Contains.Trim()}%";
            query = filter.Mode == StringFilterMode.Insensitive
                ? query.Where(x => EF.Functions.ILike(EF.Property<string>(x, propertyName), pattern))
                : query.Where(x => EF.Functions.Like(EF.Property<string>(x, propertyName), pattern));
        }

        return query;
    }

    private static IQueryable<SkiField> ApplyNumericFilter(
        IQueryable<SkiField> query,
        NumericFilter filter,
        string propertyName)
    {
        if (filter.EqualTo is { } equals)
        {
            query = query.Where(x => EF.Property<decimal>(x, propertyName) == equals);
        }
        if (filter.GreaterThan is { } gt)
        {
            query = query.Where(x => EF.Property<decimal>(x, propertyName) > gt);
        }
        if (filter.GreaterThanOrEqual is { } gte)
        {
            query = query.Where(x => EF.Property<decimal>(x, propertyName) >= gte);
        }
        if (filter.LessThan is { } lt)
        {
            query = query.Where(x => EF.Property<decimal>(x, propertyName) < lt);
        }
        if (filter.LessThanOrEqual is { } lte)
        {
            query = query.Where(x => EF.Property<decimal>(x, propertyName) <= lte);
        }

        return query;
    }

    private static IQueryable<SkiField> ApplyOrdering(
        IQueryable<SkiField> query,
        IReadOnlyList<OrderByClause>? orderBy)
    {
        if (orderBy is null || orderBy.Count == 0)
        {
            return query.OrderBy(x => x.Name);
        }

        IOrderedQueryable<SkiField>? ordered = null;

        foreach (var clause in orderBy)
        {
            ordered = ApplySingleOrder(query, ordered, clause);
        }

        return ordered ?? query.OrderBy(x => x.Name);
    }

    private static IOrderedQueryable<SkiField>? ApplySingleOrder(
        IQueryable<SkiField> source,
        IOrderedQueryable<SkiField>? ordered,
        OrderByClause clause)
    {
        var field = clause.Field.Trim().ToLowerInvariant();
        var direction = clause.Direction;

        return (field, direction) switch
        {
            ("name", SortDirection.Asc) => ordered is null ? source.OrderBy(x => x.Name) : ordered.ThenBy(x => x.Name),
            ("name", SortDirection.Desc) => ordered is null ? source.OrderByDescending(x => x.Name) : ordered.ThenByDescending(x => x.Name),
            ("countrycode", SortDirection.Asc) => ordered is null ? source.OrderBy(x => x.CountryCode) : ordered.ThenBy(x => x.CountryCode),
            ("countrycode", SortDirection.Desc) => ordered is null ? source.OrderByDescending(x => x.CountryCode) : ordered.ThenByDescending(x => x.CountryCode),
            ("region", SortDirection.Asc) => ordered is null ? source.OrderBy(x => x.Region) : ordered.ThenBy(x => x.Region),
            ("region", SortDirection.Desc) => ordered is null ? source.OrderByDescending(x => x.Region) : ordered.ThenByDescending(x => x.Region),
            ("fulldaypassprice", SortDirection.Asc) => ordered is null ? source.OrderBy(x => x.FullDayPassPrice) : ordered.ThenBy(x => x.FullDayPassPrice),
            ("fulldaypassprice", SortDirection.Desc) => ordered is null ? source.OrderByDescending(x => x.FullDayPassPrice) : ordered.ThenByDescending(x => x.FullDayPassPrice),
            ("currency", SortDirection.Asc) => ordered is null ? source.OrderBy(x => x.Currency) : ordered.ThenBy(x => x.Currency),
            ("currency", SortDirection.Desc) => ordered is null ? source.OrderByDescending(x => x.Currency) : ordered.ThenByDescending(x => x.Currency),
            ("nearesttown", SortDirection.Asc) => ordered is null ? source.OrderBy(x => x.NearestTown) : ordered.ThenBy(x => x.NearestTown),
            ("nearesttown", SortDirection.Desc) => ordered is null ? source.OrderByDescending(x => x.NearestTown) : ordered.ThenByDescending(x => x.NearestTown),
            ("createdat", SortDirection.Asc) => ordered is null ? source.OrderBy(x => x.CreatedAt) : ordered.ThenBy(x => x.CreatedAt),
            ("createdat", SortDirection.Desc) => ordered is null ? source.OrderByDescending(x => x.CreatedAt) : ordered.ThenByDescending(x => x.CreatedAt),
            ("updatedat", SortDirection.Asc) => ordered is null ? source.OrderBy(x => x.UpdatedAt) : ordered.ThenBy(x => x.UpdatedAt),
            ("updatedat", SortDirection.Desc) => ordered is null ? source.OrderByDescending(x => x.UpdatedAt) : ordered.ThenByDescending(x => x.UpdatedAt),
            _ => ordered
        };
    }
}

