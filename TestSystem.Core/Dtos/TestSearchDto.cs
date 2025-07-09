namespace TestSystem.Core.Dtos;

public record TestSearchDto(
    string? SearchTerm,
    List<string>? TestTypes,
    List<string>? Statuses,
    Guid? CompanyId,
    DateTime? CreatedAfter,
    DateTime? CreatedBefore,
    List<string>? Tags,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize
);