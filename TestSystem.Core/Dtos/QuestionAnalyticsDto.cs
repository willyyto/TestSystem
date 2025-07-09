namespace TestSystem.Core.Dtos;

public record QuestionAnalyticsDto(
    Guid QuestionId,
    string QuestionText,
    string QuestionType,
    int TotalResponses,
    int CorrectResponses,
    double SuccessRate,
    TimeSpan AverageTimeSpent,
    List<AnswerAnalyticsDto> AnswerAnalytics
);