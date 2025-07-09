namespace TestSystem.Core.Entities;

public class Notification : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Type { get; set; } = string.Empty; // "test_assigned", "test_completed", "certificate_issued", etc.
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public string? ActionUrl { get; set; }
    
    // Base interface properties
    public bool IsActive { get; set; } = true;
    public bool IsArchived { get; set; } = false;
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}