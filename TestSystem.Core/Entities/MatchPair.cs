namespace TestSystem.Core.Entities;

public class MatchPair : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public string LeftItem { get; set; }
    public Guid LeftItemId { get; set; }
    public string RightItem { get; set; }
    public Guid RightItemId { get; set; }
    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}