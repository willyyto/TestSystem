using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Mappers;

public static class MatchPairMapper
{
    public static MatchPairDto MapToMatchPairDto(this MatchPair matchPair)
    {
        return new MatchPairDto(
            matchPair.Id,
            matchPair.LeftItem,
            matchPair.RightItem
        );
    }
}