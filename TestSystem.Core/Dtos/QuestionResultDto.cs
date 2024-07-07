namespace TestSystem.Core.Dtos;

public record QuestionResultDto(Guid Id, Guid QuestionId, bool IsCorrect, QuestionDto Question);