using TestSystem.Core.Entities;

namespace TestSystem.Core.Dtos;

public record TestDto(Guid Id, string Title, string Company, List<QuestionDto> Questions, bool IsActive);