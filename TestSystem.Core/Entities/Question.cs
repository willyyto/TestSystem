namespace TestSystem.Core.Entities;

public enum QuestionType
{
    MultipleChoice,
    TrueFalse,
    ShortAnswer,
    Essay,
    FillInTheBlank,
    Matching,
    MultipleSelect, // Allow multiple correct answers
    Ordering,       // Put items in correct order
    Numerical,      // Number input with tolerance
    Scale,          // Rating scale (1-5, 1-10, etc.)
    FileUpload      // File submission
}


public class Question : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public Guid TestId { get; set; }
    public Test Test { get; set; }
    public QuestionType Type { get; set; }
    public string Text { get; set; }
    public string? ImageUrl { get; set; } // Support for images in questions
    public string? VideoUrl { get; set; } // Support for videos
    public string? AudioUrl { get; set; } // Support for audio
    public double Weight { get; set; } = 1.0;
    public int TimeLimit { get; set; } = 0; // Time limit in seconds, 0 = no limit
    public bool IsRequired { get; set; } = true;
    public string? Explanation { get; set; } // Explanation shown after answering
    public string? Hint { get; set; } // Hint available during test
    public int DisplayOrder { get; set; } // Order of questions
    
    // Multiple choice specific
    public bool AllowMultipleAnswers { get; set; } = false;
    public bool ShuffleAnswers { get; set; } = false;
    
    // Numerical question specific
    public double? CorrectNumericalAnswer { get; set; }
    public double? NumericalTolerance { get; set; }
    public string? NumericalUnit { get; set; }
    
    // Scale question specific
    public int? ScaleMin { get; set; }
    public int? ScaleMax { get; set; }
    public string? ScaleMinLabel { get; set; }
    public string? ScaleMaxLabel { get; set; }
    
    // File upload specific
    public string? AllowedFileTypes { get; set; } // comma separated: .pdf,.doc,.jpg
    public int? MaxFileSizeKB { get; set; }
    
    // Ordering specific
    public string? OrderingInstructions { get; set; }

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    public ICollection<MatchPair> MatchPairs { get; set; } = new List<MatchPair>();
    public ICollection<OrderingItem> OrderingItems { get; set; } = new List<OrderingItem>();
    public ICollection<QuestionResult> QuestionResults { get; set; } = new List<QuestionResult>();
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}