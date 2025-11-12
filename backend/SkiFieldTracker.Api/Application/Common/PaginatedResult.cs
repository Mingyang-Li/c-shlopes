namespace SkiFieldTracker.Application.Common;

public sealed record PaginatedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int? Skip,
    int? Take);

