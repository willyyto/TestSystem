namespace TestSystem.Core.Entities;

public class User : IArchivable, IMetaData, IActive, ILockable
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string RefreshToken { get; set; }
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }
    public Guid? CompanyId { get; set; }
    public Company Company { get; set; }
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public bool IsLocked { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }


}