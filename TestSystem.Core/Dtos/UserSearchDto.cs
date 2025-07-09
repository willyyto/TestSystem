namespace TestSystem.Core.Dtos;

public record UserSearchDto(
    string? SearchTerm,
    List<string>? Roles,
    Guid? CompanyId,
    bool? IsActive,
    bool? EmailVerified,
    DateTime? LastLoginAfter,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize
);