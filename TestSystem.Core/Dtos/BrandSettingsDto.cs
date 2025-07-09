namespace TestSystem.Core.Dtos;

public record BrandingSettingsDto(
    string? LogoUrl,
    string? PrimaryColor,
    string? SecondaryColor,
    string? FontFamily,
    string? CustomCss
);