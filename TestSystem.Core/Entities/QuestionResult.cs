namespace TestSystem.Core.Entities;

public class QuestionResult : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public Guid TestResultId { get; set; }
    public Guid QuestionId { get; set; }
    public string Answer { get; set; }
    public bool IsCorrect { get; set; }
    public TestResult TestResult { get; set; }
    public Question Question { get; set; }
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}