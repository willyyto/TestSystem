namespace TestSystem.Core.Entities;

public class TestResult : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid TestId { get; set; }
    public Test Test { get; set; }
    public Guid? TestAttemptId { get; set; }
    public TestAttempt? TestAttempt { get; set; }
    public DateTime CompletedDate { get; set; }
    public int Score { get; set; } // Percentage score
    public double RawScore { get; set; } // Raw points earned
    public double MaxPossibleScore { get; set; } // Maximum points possible
    public string Grade { get; set; } = string.Empty; // Letter grade if applicable
    public bool Passed { get; set; }
    public TimeSpan TimeSpent { get; set; }
    public int QuestionsAnswered { get; set; }
    public int QuestionsCorrect { get; set; }
    public int QuestionsSkipped { get; set; }
    public string? Comments { get; set; } // Instructor comments
    public bool IsManuallyGraded { get; set; } = false;
    public Guid? GradedBy { get; set; } // User ID who graded manually
    public DateTime? GradedAt { get; set; }
    public string? CertificateUrl { get; set; } // Link to generated certificate
    public ICollection<QuestionResult> QuestionResults { get; set; } = new List<QuestionResult>();
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}