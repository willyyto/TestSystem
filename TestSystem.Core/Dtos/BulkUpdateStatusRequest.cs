namespace TestSystem.Core.Dtos;

public record BulkUpdateStatusRequest(List<Guid> UserIds, bool IsActive);