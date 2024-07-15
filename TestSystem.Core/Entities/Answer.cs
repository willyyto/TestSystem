namespace TestSystem.Core.Entities;

public class Answer : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public bool IsFillInTheBlank { get; set; }
    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}