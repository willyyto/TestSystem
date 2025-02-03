namespace TestSystem.Core.Entities;

public class TestResult : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid TestId { get; set; }
    public Test Test { get; set; }
    public DateTime CompletedDate { get; set; }
    public int Score { get; set; }
    public ICollection<QuestionResult> QuestionResults { get; set; }
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}