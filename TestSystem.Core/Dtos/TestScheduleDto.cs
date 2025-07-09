namespace TestSystem.Core.Dtos;

public record TestScheduleDto(
    Guid Id,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string? TimeZone,
    bool IsRecurring,
    string? RecurrencePattern,
    int MaxParticipants
);