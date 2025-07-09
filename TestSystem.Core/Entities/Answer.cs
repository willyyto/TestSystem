namespace TestSystem.Core.Entities;

public class Answer : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public bool IsFillInTheBlank { get; set; }
    public string? ImageUrl { get; set; } // Support for images in answers
    public string? Explanation { get; set; } // Explanation for this answer
    public double Points { get; set; } = 1.0; // Points awarded for this answer
    public bool IsCaseSensitive { get; set; } = false; // For text answers
    public string? AcceptableAnswers { get; set; } // JSON array of acceptable variations
    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}
