namespace TestSystem.Core.Dtos;

public record CreateAnswerDto(
    string Text,
    bool IsCorrect,
    bool IsFillInTheBlank,
    string? ImageUrl,
    string? Explanation,
    double Points,
    bool IsCaseSensitive,
    string? AcceptableAnswers
);