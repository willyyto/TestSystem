namespace TestSystem.Core.Dtos;

public record TestResultDto(
    Guid Id,
    Guid UserId,
    Guid TestId,
    DateTime AttemptDate,
    int Score,
    TestDto Test,
    List<QuestionResultDto> QuestionResults);