using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Mappers;

public static class OrderingItemMapper
{
    public static OrderingItemDto MapToOrderingItemDto(this OrderingItem orderingItem)
    {
        return new OrderingItemDto(
            orderingItem.Id,
            orderingItem.Text,
            orderingItem.CorrectOrder
        );
    }

    public static OrderingItem MapToOrderingItem(this CreateOrderingItemDto dto, Guid questionId)
    {
        return new OrderingItem
        {
            Id = Guid.NewGuid(),
            QuestionId = questionId,
            Text = dto.Text,
            CorrectOrder = dto.CorrectOrder,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };
    }
}