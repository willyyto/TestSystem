namespace TestSystem.Core.Dtos;

public record UserDto(
    Guid Id,
    string Username,
    string Name,
    string Email,
    string Role,
    string? FirstName,
    string? LastName,
    string? ProfilePictureUrl,
    string? Phone,
    string? Department,
    string? JobTitle,
    DateTime? LastLoginAt,
    bool EmailVerified,
    bool TwoFactorEnabled,
    string? Timezone,
    string? Language,
    bool NotificationEmailEnabled,
    bool NotificationSmsEnabled,
    CompanyDto? Company,
    bool IsArchived,
    bool IsActive,
    bool IsLocked
);