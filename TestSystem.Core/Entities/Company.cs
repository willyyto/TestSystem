namespace TestSystem.Core.Entities;

public class Company : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Website { get; set; }
    public string? LogoUrl { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? ContactPerson { get; set; }
    
    // Subscription and limits
    public string SubscriptionTier { get; set; } = "Free"; // Free, Pro, Enterprise
    public DateTime? SubscriptionStart { get; set; }
    public DateTime? SubscriptionEnd { get; set; }
    public int MaxUsers { get; set; } = 10;
    public int MaxTests { get; set; } = 5;
    public int MaxQuestionsPerTest { get; set; } = 50;
    public bool CustomBrandingEnabled { get; set; } = false;
    public bool AdvancedReportsEnabled { get; set; } = false;
    public bool ApiAccessEnabled { get; set; } = false;
    public long StorageLimitMB { get; set; } = 100;
    public long StorageUsedMB { get; set; } = 0;
    
    // Settings
    public string? CustomCss { get; set; }
    public string? CustomDomain { get; set; }
    public string? SmtpSettings { get; set; } // JSON for email settings
    public string? Settings { get; set; } // JSON for other company settings
    
    public ICollection<Test> Tests { get; set; } = new List<Test>();
    public ICollection<User> Users { get; set; } = new List<User>();
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}