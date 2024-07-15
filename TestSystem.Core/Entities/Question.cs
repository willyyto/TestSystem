namespace TestSystem.Core.Entities;

public enum QuestionType
{
    MultipleChoice,
    TrueFalse,
    ShortAnswer,
    Essay,
    FillInTheBlank,
    Matching
}

public class Question : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public Guid TestId { get; set; }
    public Test Test { get; set; }
    public QuestionType Type { get; set; }
    public string Text { get; set; }

    public double Weight { get; set; } = 1.0;
    public ICollection<MatchPair> MatchPairs { get; set; }
    public ICollection<Answer> Answers { get; set; }
    public ICollection<QuestionResult> QuestionResults { get; set; }
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}