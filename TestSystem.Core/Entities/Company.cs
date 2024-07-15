namespace TestSystem.Core.Entities;

public class Company : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Test> Tests { get; set; } = new List<Test>();
    public ICollection<User> Users { get; set; } = new List<User>();
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}