namespace TestSystem.Core.Entities;

public class TestSchedule : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public Guid TestId { get; set; }
    public Test Test { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? TimeZone { get; set; }
    public bool IsRecurring { get; set; } = false;
    public string? RecurrencePattern { get; set; } // JSON for recurrence rules
    public int MaxParticipants { get; set; } = 0; // 0 = unlimited
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}