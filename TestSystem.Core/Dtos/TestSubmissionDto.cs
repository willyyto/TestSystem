namespace TestSystem.Core.Dtos;

public record TestSubmissionDto(Guid Id, Dictionary<Guid, string> Answers);