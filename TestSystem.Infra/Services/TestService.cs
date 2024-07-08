using Microsoft.EntityFrameworkCore;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Infra.DataServices;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Infra.Services;

[InstanceScopedService]
public class TestService : ITestService
{
    private readonly ITestSystemDbContextAsync _tsDbContext;

    public TestService(ITestSystemDbContextAsync tsDbContext)
    {
        _tsDbContext = tsDbContext;
    }

    public async Task<int?> SubmitQuiz(CancellationToken ct, TestSubmissionDto submission, Guid userId)
    {
        var test = await _tsDbContext.Tests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.Id == submission.TestId, ct);

        if (test == null) return null;

        // Process answers and calculate score
        var score = 0;

        foreach (var question in test.Questions)
            if (submission.Answers.TryGetValue(question.Id, out var answer))
            {
                if (question.Type == QuestionType.MultipleChoice || question.Type == QuestionType.TrueFalse)
                {
                    var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);
                    if (correctAnswer != null && correctAnswer.Text == answer) score++;
                }
                else if (question.Type == QuestionType.ShortAnswer)
                {
                    // Add custom logic for short answer grading if needed
                }
            }

        // Save test result
        var testResult = new TestResult
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TestId = test.Id,
            AttemptDate = DateTime.UtcNow,
            Score = score,
            QuestionResults = test.Questions.Select(q => new QuestionResult
            {
                Id = Guid.NewGuid(),
                QuestionId = q.Id,
                Answer = submission.Answers.ContainsKey(q.Id) ? submission.Answers[q.Id] : string.Empty,
                IsCorrect = submission.Answers.TryGetValue(q.Id, out var ans) &&
                            q.Answers.Any(a => a.IsCorrect && a.Text == ans)
            }).ToList()
        };

        _tsDbContext.TestResults.Add(testResult);
        await _tsDbContext.SaveChangesAsync(ct);

        return score;
    }
}
