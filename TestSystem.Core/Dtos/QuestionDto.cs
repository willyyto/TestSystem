namespace TestSystem.Core.Dtos;

public record QuestionDto(Guid Id, string Text, string Type, List<AnswerDto> Answers);