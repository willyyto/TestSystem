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
            testResult.AttemptDate,
            testResult.Score,
            testResult.Test.MapToTestDto(),
            testResult.QuestionResults.Select(qr => qr.MapToQuestionResultDto()).ToList()
        );
    }
}