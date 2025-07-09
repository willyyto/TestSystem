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
            answer.IsFillInTheBlank,
            answer.ImageUrl,
            answer.Explanation,
            answer.Points,
            answer.IsCaseSensitive,
            answer.AcceptableAnswers
        );
    }

    public static Answer MapToAnswer(this CreateAnswerDto dto, Guid questionId)
    {
        return new Answer
        {
            Id = Guid.NewGuid(),
            QuestionId = questionId,
            Text = dto.Text,
            IsCorrect = dto.IsCorrect,
            IsFillInTheBlank = dto.IsFillInTheBlank,
            ImageUrl = dto.ImageUrl,
            Explanation = dto.Explanation,
            Points = dto.Points,
            IsCaseSensitive = dto.IsCaseSensitive,
            AcceptableAnswers = dto.AcceptableAnswers,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };
    }
}