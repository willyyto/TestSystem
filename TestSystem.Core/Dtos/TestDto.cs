namespace TestSystem.Core.Dtos;

public record TestDto(Guid Id, string Title, List<QuestionDto> Questions);