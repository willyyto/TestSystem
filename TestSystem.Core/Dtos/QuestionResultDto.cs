namespace TestSystem.Core.Dtos;

public record QuestionResultDto(Guid Id, Guid QuestionId, string Answer, bool IsCorrect, QuestionDto Question);