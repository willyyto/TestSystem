namespace TestSystem.Core.Dtos;

public record BulkTestAssignmentDto(
    List<Guid> UserIds,
    List<Guid> TestIds,
    DateTime? AvailableFrom,
    DateTime? AvailableUntil,
    bool SendNotification
);