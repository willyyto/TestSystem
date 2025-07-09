namespace TestSystem.Core.Dtos;

public record OrderingItemDto(
    Guid Id,
    string Text,
    int CorrectOrder
);