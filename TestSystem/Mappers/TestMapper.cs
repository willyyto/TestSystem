using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Mappers;

public static class TestMapper
{
    public static TestDto MapToTestDto(this Test test)
    {
        return new TestDto(
            test.Id,
            test.Title,
            test.Company.Name,
            test.Questions.Select(i => i.MapToQuestionDto()).ToList(),
            test.IsActive
        );
    }
}