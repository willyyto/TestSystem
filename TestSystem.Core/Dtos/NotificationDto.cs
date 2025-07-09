namespace TestSystem.Core.Dtos;

public record NotificationDto(
    Guid Id,
    Guid UserId,
    string Type, // "test_assigned", "test_completed", "certificate_issued", etc.
    string Title,
    string Message,
    bool IsRead,
    DateTime CreatedAt,
    string? ActionUrl
);