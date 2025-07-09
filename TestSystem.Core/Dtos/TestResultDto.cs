namespace TestSystem.Core.Dtos;

public record TestResultDto(
    Guid Id,
    Guid UserId,
    Guid TestId,
    DateTime CompletedDate,
    int Score,
    double RawScore,
    double MaxPossibleScore,
    string Grade,
    bool Passed,
    TimeSpan TimeSpent,
    int QuestionsAnswered,
    int QuestionsCorrect,
    int QuestionsSkipped,
    string? Comments,
    bool IsManuallyGraded,
    Guid? GradedBy,
    DateTime? GradedAt,
    string? CertificateUrl,
    TestDto Test,
    List<QuestionResultDto> QuestionResults
);