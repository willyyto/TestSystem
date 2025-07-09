namespace TestSystem.Core.Entities;

public class QuestionResult : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public Guid TestResultId { get; set; }
    public Guid QuestionId { get; set; }
    public string Answer { get; set; } // JSON for complex answers
    public bool IsCorrect { get; set; }
    public double PointsEarned { get; set; }
    public double MaxPoints { get; set; }
    public TimeSpan? TimeSpent { get; set; }
    public bool IsSkipped { get; set; } = false;
    public bool RequiresManualGrading { get; set; } = false;
    public string? InstructorFeedback { get; set; }
    public string? FileSubmissionPath { get; set; } // For file upload questions
    public TestResult TestResult { get; set; }
    public Question Question { get; set; }
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}