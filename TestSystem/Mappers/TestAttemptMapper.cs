using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Mappers;

public static class TestAttemptMapper
{
    public static TestAttemptDto MapToTestAttemptDto(this TestAttempt attempt)
    {
        return new TestAttemptDto(
            attempt.Id,
            attempt.TestId,
            attempt.UserId,
            attempt.StartedAt,
            attempt.CompletedAt,
            attempt.TimeSpent,
            attempt.IsCompleted,
            attempt.IsAbandoned,
            attempt.AttemptNumber,
            attempt.TabSwitchCount
        );
    }
}
