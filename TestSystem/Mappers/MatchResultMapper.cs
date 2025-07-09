using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Mappers;

public static class MatchPairMapper
{
    public static MatchPairDto MapToMatchPairDto(this MatchPair matchPair)
    {
        return new MatchPairDto(
            matchPair.Id,
            matchPair.LeftItemId,
            matchPair.LeftItem,
            matchPair.RightItemId,
            matchPair.RightItem
        );
    }

    public static MatchPair MapToMatchPair(this CreateMatchPairDto dto, Guid questionId)
    {
        return new MatchPair
        {
            Id = Guid.NewGuid(),
            QuestionId = questionId,
            LeftItem = dto.LeftItem,
            LeftItemId = Guid.NewGuid(),
            RightItem = dto.RightItem,
            RightItemId = Guid.NewGuid(),
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };
    }
}
