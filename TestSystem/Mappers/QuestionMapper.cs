using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Mappers;

public static class QuestionMapper
{
    public static QuestionDto MapToQuestionDto(this Question question)
    {
        return new QuestionDto(
            question.Id,
            question.Text,
            question.Type.ToString(),
            question.Weight,
            question.Answers.Select(a => a.MapToAnswerDto()).ToList(),
            question.MatchPairs?.Select(mp => mp.MapToMatchPairDto()).ToList() ?? new List<MatchPairDto>()
        );
    }
}