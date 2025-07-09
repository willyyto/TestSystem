namespace TestSystem.Core.Dtos;

public record CompanySettingsDto(
    Guid CompanyId,
    string? CustomCss,
    string? CustomDomain,
    EmailSettingsDto? EmailSettings,
    SecuritySettingsDto? SecuritySettings,
    BrandingSettingsDto? BrandingSettings
);