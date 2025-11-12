using System.Text.Json.Serialization;

namespace SkiFieldTracker.Application.SkiFields.Models;

public sealed class FindManyRequest
{
    [JsonPropertyName("where")]
    public SkiFieldFilter? Where { get; init; }

    [JsonPropertyName("orderBy")]
    public IReadOnlyList<OrderByClause>? OrderBy { get; init; }

    [JsonPropertyName("skip")]
    public int? Skip { get; init; }

    [JsonPropertyName("take")]
    public int? Take { get; init; }
}

public sealed class SkiFieldFilter
{
    [JsonPropertyName("name")]
    public StringFilter? Name { get; init; }

    [JsonPropertyName("country")]
    public StringFilter? Country { get; init; }

    [JsonPropertyName("region")]
    public StringFilter? Region { get; init; }

    [JsonPropertyName("adultFullDayPassUsd")]
    public NumericFilter? AdultFullDayPassUsd { get; init; }
}

public sealed class StringFilter
{
    [JsonPropertyName("equals")]
    public string? EqualTo { get; init; }

    [JsonPropertyName("contains")]
    public string? Contains { get; init; }

    [JsonPropertyName("mode")]
    public StringFilterMode Mode { get; init; } = StringFilterMode.Insensitive;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StringFilterMode
{
    Sensitive,
    Insensitive
}

public sealed class NumericFilter
{
    [JsonPropertyName("equals")]
    public decimal? EqualTo { get; init; }

    [JsonPropertyName("gt")]
    public decimal? GreaterThan { get; init; }

    [JsonPropertyName("gte")]
    public decimal? GreaterThanOrEqual { get; init; }

    [JsonPropertyName("lt")]
    public decimal? LessThan { get; init; }

    [JsonPropertyName("lte")]
    public decimal? LessThanOrEqual { get; init; }
}

public sealed class OrderByClause
{
    [JsonPropertyName("field")]
    public string Field { get; init; } = string.Empty;

    [JsonPropertyName("direction")]
    public SortDirection Direction { get; init; } = SortDirection.Asc;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortDirection
{
    Asc,
    Desc
}

