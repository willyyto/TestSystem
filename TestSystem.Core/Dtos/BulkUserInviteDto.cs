namespace TestSystem.Core.Dtos;

public record BulkUserInviteDto(
    List<string> EmailAddresses,
    Guid CompanyId,
    string Role,
    List<Guid> AssignedTestIds
);