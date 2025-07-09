namespace TestSystem.Core.Dtos;

public record AnswerDto(
    Guid Id,
    string Text,
    bool IsCorrect,
    bool IsFillInTheBlank,
    string? ImageUrl,
    string? Explanation,
    double Points,
    bool IsCaseSensitive,
    string? AcceptableAnswers
);