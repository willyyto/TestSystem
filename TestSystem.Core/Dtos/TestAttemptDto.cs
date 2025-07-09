namespace TestSystem.Core.Dtos;

public record TestAttemptDto(
    Guid Id,
    Guid TestId,
    Guid UserId,
    DateTime StartedAt,
    DateTime? CompletedAt,
    TimeSpan? TimeSpent,
    bool IsCompleted,
    bool IsAbandoned,
    int AttemptNumber,
    int TabSwitchCount
);