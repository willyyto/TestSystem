using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Mappers;

public static class TestMapper
{
    public static TestDto MapToTestDto(this Test test)
    {
        return new TestDto(
            test.Id,
            test.Name,
            test.Company.Name,
            test.StartDate,
            test.EndDate,
            test.Duration,
            test.PassMark,
            test.IsTimed,
            test.ShuffleQuestions,
            test.MaximumAttempts,
            test.Visibility.ToString(),
            test.TestType.ToString(),
            test.Instructions,
            test.Feedback.ToString(),
            test.TestAccessControl.ToString(),
            test.GradingScheme.ToString(),
            test.RetakePolicy.MapToRetakePolicyDto(),
            test.Questions.Select(q => q.MapToQuestionDto()).ToList(),
            test.IsArchived,
            test.IsActive
        );
    }
}