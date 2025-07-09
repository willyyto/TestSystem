namespace TestSystem.Core.Dtos;

public record RecentActivityDto(
    string ActivityType,
    string Description,
    DateTime Timestamp,
    Guid? UserId,
    string? UserName
);