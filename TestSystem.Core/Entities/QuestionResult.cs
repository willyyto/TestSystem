namespace TestSystem.Core.Entities;

public class QuestionResult
{
    public Guid Id { get; set; }
    public Guid TestResultId { get; set; }
    public Guid QuestionId { get; set; }
    public bool IsCorrect { get; set; }
    public TestResult TestResult { get; set; }
    public Question Question { get; set; }
}