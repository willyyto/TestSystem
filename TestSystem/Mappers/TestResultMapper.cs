using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Mappers;

public static class TestResultMapper
{
    public static TestResultDto MapToTestResultDto(this TestResult testResult)
    {
        return new TestResultDto(
            testResult.Id,
            testResult.UserId,
            testResult.TestId,
            testResult.CompletedDate,
            testResult.Score,
            testResult.RawScore,
            testResult.MaxPossibleScore,
            testResult.Grade,
            testResult.Passed,
            testResult.TimeSpent,
            testResult.QuestionsAnswered,
            testResult.QuestionsCorrect,
            testResult.QuestionsSkipped,
            testResult.Comments,
            testResult.IsManuallyGraded,
            testResult.GradedBy,
            testResult.GradedAt,
            testResult.CertificateUrl,
            testResult.Test.MapToTestDto(),
            testResult.QuestionResults?.Select(qr => qr.MapToQuestionResultDto()).ToList() ?? new List<QuestionResultDto>()
        );
    }
}