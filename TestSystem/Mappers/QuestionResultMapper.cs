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
            questionResult.Answer,
            questionResult.IsCorrect,
            questionResult.PointsEarned,
            questionResult.MaxPoints,
            questionResult.TimeSpent,
            questionResult.IsSkipped,
            questionResult.RequiresManualGrading,
            questionResult.InstructorFeedback,
            questionResult.FileSubmissionPath,
            questionResult.Question.MapToQuestionDto()
        );
    }
}