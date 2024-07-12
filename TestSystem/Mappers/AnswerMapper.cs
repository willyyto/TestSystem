using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Mappers;

public static class AnswerMapper
{
    public static AnswerDto MapToAnswerDto(this Answer answer)
    {
        return new AnswerDto(
            answer.Id,
            answer.Text,
            answer.IsCorrect,
            answer.IsFillInTheBlank
        );
    }
}