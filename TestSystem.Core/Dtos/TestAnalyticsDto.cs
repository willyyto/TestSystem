namespace TestSystem.Core.Dtos;

public record TestAnalyticsDto(
    Guid TestId,
    string TestName,
    int TotalAttempts,
    int CompletedAttempts,
    int PassedAttempts,
    double AverageScore,
    double PassRate,
    TimeSpan AverageCompletionTime,
    List<QuestionAnalyticsDto> QuestionAnalytics
);