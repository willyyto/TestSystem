using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Mappers;

public static class TestScheduleMapper
{
    public static TestScheduleDto MapToTestScheduleDto(this TestSchedule schedule)
    {
        return new TestScheduleDto(
            schedule.Id,
            schedule.StartDateTime,
            schedule.EndDateTime,
            schedule.TimeZone,
            schedule.IsRecurring,
            schedule.RecurrencePattern,
            schedule.MaxParticipants
        );
    }

    public static TestSchedule MapToTestSchedule(this TestScheduleDto dto, Guid testId)
    {
        return new TestSchedule
        {
            Id = Guid.NewGuid(),
            TestId = testId,
            StartDateTime = dto.StartDateTime,
            EndDateTime = dto.EndDateTime,
            TimeZone = dto.TimeZone,
            IsRecurring = dto.IsRecurring,
            RecurrencePattern = dto.RecurrencePattern,
            MaxParticipants = dto.MaxParticipants,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };
    }
}