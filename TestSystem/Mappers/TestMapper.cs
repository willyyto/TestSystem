using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Core.Migrations;

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
            test.Questions.Select(i => i.MapToQuestionDto()).ToList(),
            test.IsArchived,
            test.IsActive
        );
    }
}