namespace TestSystem.Core.Dtos;

public record AnswerAnalyticsDto(
    Guid AnswerId,
    string AnswerText,
    int SelectionCount,
    double SelectionPercentage,
    bool IsCorrect
);