namespace TestSystem.Core.Entities;

public class TestAttempt : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public Guid TestId { get; set; }
    public Test Test { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public TimeSpan? TimeSpent { get; set; }
    public bool IsCompleted { get; set; } = false;
    public bool IsAbandoned { get; set; } = false;
    public int AttemptNumber { get; set; }
    public string? UserAgent { get; set; }
    public string? IpAddress { get; set; }
    public int TabSwitchCount { get; set; } = 0;
    public string? ProctorData { get; set; } // JSON for proctoring information
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}