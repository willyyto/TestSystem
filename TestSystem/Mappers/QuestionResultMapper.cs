using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Mappers;

public static class QuestionResultMapper
{
    public static QuestionResultDto MapToQuestionResultDto(this QuestionResult questionResult)
    {
        return new QuestionResultDto(
            questionResult.Id,
            questionResult.QuestionId,
            questionResult.IsCorrect,
            questionResult.Question.MapToQuestionDto()
        );
    }
}