namespace TestSystem.Core.Dtos;

public record SecuritySettingsDto(
    bool RequireTwoFactor,
    int PasswordMinLength,
    bool RequireUppercase,
    bool RequireLowercase,
    bool RequireNumbers,
    bool RequireSpecialChars,
    int SessionTimeoutMinutes,
    bool AllowMultipleSessions
);