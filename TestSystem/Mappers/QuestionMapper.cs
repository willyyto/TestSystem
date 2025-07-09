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
            question.TimeLimit,
            question.IsRequired,
            question.ImageUrl,
            question.VideoUrl,
            question.AudioUrl,
            question.Explanation,
            question.Hint,
            question.DisplayOrder,
            question.AllowMultipleAnswers,
            question.ShuffleAnswers,
            question.CorrectNumericalAnswer,
            question.NumericalTolerance,
            question.NumericalUnit,
            question.ScaleMin,
            question.ScaleMax,
            question.ScaleMinLabel,
            question.ScaleMaxLabel,
            question.AllowedFileTypes,
            question.MaxFileSizeKB,
            question.OrderingInstructions,
            question.Answers?.Select(a => a.MapToAnswerDto()).ToList() ?? new List<AnswerDto>(),
            question.MatchPairs?.Select(mp => mp.MapToMatchPairDto()).ToList() ?? new List<MatchPairDto>(),
            question.OrderingItems?.Select(oi => oi.MapToOrderingItemDto()).ToList() ?? new List<OrderingItemDto>()
        );
    }

    public static Question MapToQuestion(this CreateQuestionDto dto)
    {
        var question = new Question
        {
            Id = Guid.NewGuid(),
            TestId = dto.TestId,
            Text = dto.Text,
            Type = Enum.Parse<QuestionType>(dto.Type),
            Weight = dto.Weight,
            TimeLimit = dto.TimeLimit,
            IsRequired = dto.IsRequired,
            ImageUrl = dto.ImageUrl,
            VideoUrl = dto.VideoUrl,
            AudioUrl = dto.AudioUrl,
            Explanation = dto.Explanation,
            Hint = dto.Hint,
            DisplayOrder = dto.DisplayOrder,
            AllowMultipleAnswers = dto.AllowMultipleAnswers,
            ShuffleAnswers = dto.ShuffleAnswers,
            CorrectNumericalAnswer = dto.CorrectNumericalAnswer,
            NumericalTolerance = dto.NumericalTolerance,
            NumericalUnit = dto.NumericalUnit,
            ScaleMin = dto.ScaleMin,
            ScaleMax = dto.ScaleMax,
            ScaleMinLabel = dto.ScaleMinLabel,
            ScaleMaxLabel = dto.ScaleMaxLabel,
            AllowedFileTypes = dto.AllowedFileTypes,
            MaxFileSizeKB = dto.MaxFileSizeKB,
            OrderingInstructions = dto.OrderingInstructions,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        // Map answers
        if (dto.Answers?.Any() == true)
        {
            question.Answers = dto.Answers.Select(a => a.MapToAnswer(question.Id)).ToList();
        }

        // Map match pairs
        if (dto.MatchPairs?.Any() == true)
        {
            question.MatchPairs = dto.MatchPairs.Select(mp => mp.MapToMatchPair(question.Id)).ToList();
        }

        // Map ordering items
        if (dto.OrderingItems?.Any() == true)
        {
            question.OrderingItems = dto.OrderingItems.Select(oi => oi.MapToOrderingItem(question.Id)).ToList();
        }

        return question;
    }
}