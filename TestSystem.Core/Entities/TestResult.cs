namespace TestSystem.Core.Entities;

public class TestResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid TestId { get; set; }
    public DateTime AttemptDate { get; set; }
    public int Score { get; set; }
    public ICollection<QuestionResult> QuestionResults { get; set; }
    public Test Test { get; set; }
}