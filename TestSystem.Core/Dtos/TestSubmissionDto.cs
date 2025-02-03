namespace TestSystem.Core.Dtos;

public class TestSubmissionDto
{
    public Guid TestId { get; set; }
    public Dictionary<Guid, string> Answers { get; set; }
    public Dictionary<Guid, Dictionary<Guid, string>> MatchingAnswers { get; set; } = new();
}