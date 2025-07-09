namespace TestSystem.Core.Dtos;

public record UpdateCompanyDto(
    string Name,
    string? Description,
    string? Website,
    string? LogoUrl,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    string? Phone,
    string? Email,
    string? ContactPerson,
    string SubscriptionTier,
    int MaxUsers,
    int MaxTests,
    int MaxQuestionsPerTest,
    bool CustomBrandingEnabled,
    bool AdvancedReportsEnabled,
    bool ApiAccessEnabled,
    long StorageLimitMB,
    string? CustomDomain
);