namespace TestSystem.Core.Dtos;

public record DuplicateTestRequest(string NewName, Guid? TargetCompanyId = null);