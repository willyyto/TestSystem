namespace TestSystem.Core.Dtos;

public record CompanyDto(Guid Id, string Name, bool isActive, bool isArchived);