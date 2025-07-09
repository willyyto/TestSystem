namespace TestSystem.Core.Dtos;

public class TestSubmissionDto
{
    public Guid TestId { get; set; }
    public Dictionary<Guid, string> Answers { get; set; } = new();
    public Dictionary<Guid, Dictionary<Guid, string>> MatchingAnswers { get; set; } = new();
    public Dictionary<Guid, List<string>> OrderingAnswers { get; set; } = new();
    public Dictionary<Guid, double> NumericalAnswers { get; set; } = new();
    public Dictionary<Guid, int> ScaleAnswers { get; set; } = new();
    public Dictionary<Guid, List<Guid>> MultipleSelectAnswers { get; set; } = new();
    public Dictionary<Guid, string> FileSubmissions { get; set; } = new(); // File paths
    public Dictionary<Guid, TimeSpan> QuestionTimes { get; set; } = new(); // Time spent per question
    public string? Password { get; set; } // Test password if required
}