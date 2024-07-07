namespace TestSystem.Core.Dtos;

public class TestSubmissionDto
{
    public Guid TestId { get; set; }
    public Dictionary<Guid, string> Answers { get; set; }
}