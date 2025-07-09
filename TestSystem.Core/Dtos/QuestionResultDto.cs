namespace TestSystem.Core.Dtos;

public record QuestionResultDto(
    Guid Id,
    Guid QuestionId,
    string Answer,
    bool IsCorrect,
    double PointsEarned,
    double MaxPoints,
    TimeSpan? TimeSpent,
    bool IsSkipped,
    bool RequiresManualGrading,
    string? InstructorFeedback,
    string? FileSubmissionPath,
    QuestionDto Question
);