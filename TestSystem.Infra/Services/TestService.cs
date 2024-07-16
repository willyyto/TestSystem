using System.Text.Json;
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
            .Include(t => t.Questions)
            .ThenInclude(q => q.MatchPairs)
            .FirstOrDefaultAsync(t => t.Id == submission.TestId);

        if (test == null) return null;

        // Process answers and calculate score with weights
        double totalWeight = 0;
        double weightedScore = 0;

        foreach (var question in test.Questions)
        {
            totalWeight += question.Weight;
            var isCorrect = false;

            if (question.Type == QuestionType.MultipleChoice || question.Type == QuestionType.TrueFalse)
            {
                if (submission.Answers.TryGetValue(question.Id, out var answer))
                {
                    var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);
                    if (correctAnswer != null && correctAnswer.Id == Guid.Parse(answer)) isCorrect = true;
                }
            }
            else if (question.Type == QuestionType.FillInTheBlank || question.Type == QuestionType.ShortAnswer ||
                     question.Type == QuestionType.Essay)
            {
                if (submission.Answers.TryGetValue(question.Id, out var answer))
                {
                    var correctAnswer = question.Answers.FirstOrDefault(a => a.IsFillInTheBlank);
                    if (correctAnswer != null && correctAnswer.Text == answer) isCorrect = true;
                }
            }
            else if (question.Type == QuestionType.Matching)
            {
                if (submission.MatchingAnswers.TryGetValue(question.Id, out var submittedPairs))
                {
                    isCorrect = true;
                    foreach (var pair in question.MatchPairs)
                        if (!submittedPairs.TryGetValue(pair.LeftItemId, out var rightItemId) ||
                            rightItemId != pair.RightItemId.ToString())
                        {
                            isCorrect = false;
                            break;
                        }
                }
            }

            if (isCorrect) weightedScore += question.Weight;

            var questionResult = new QuestionResult
            {
                Id = Guid.NewGuid(),
                QuestionId = question.Id,
                IsCorrect = isCorrect,
                Answer = question.Type == QuestionType.Matching
                    ? JsonSerializer.Serialize(submission.MatchingAnswers[question.Id])
                    : submission.Answers[question.Id]
            };

            _tsDbContext.QuestionResults.Add(questionResult);
        }

        // Calculate the final score as a percentage
        var scorePercentage = weightedScore / totalWeight * 100;

        // Save test result
        var testResult = new TestResult
        {
            Id = Guid.NewGuid(),
            UserId = userId, // Set this to the actual user ID if needed
            TestId = test.Id,
            CompletedDate = DateTime.UtcNow,
            Score = (int) scorePercentage,
            QuestionResults = _tsDbContext.QuestionResults.Local.ToList()
        };

        _tsDbContext.TestResults.Add(testResult);
        await _tsDbContext.SaveChangesAsync(ct);

        return (int) scorePercentage;
    }
}