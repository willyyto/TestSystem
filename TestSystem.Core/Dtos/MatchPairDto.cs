namespace TestSystem.Core.Dtos;

public record MatchPairDto(Guid Id, Guid LeftItemId, string LeftItem, Guid RightItemId, string RightItem);