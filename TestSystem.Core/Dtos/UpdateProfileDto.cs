namespace TestSystem.Core.Dtos;

public record UpdateProfileDto(
    string Name,
    string? FirstName,
    string? LastName,
    string? Phone,
    string? Timezone,
    string? Language,
    bool NotificationEmailEnabled,
    bool NotificationSmsEnabled
);