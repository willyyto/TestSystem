using TestSystem.Core.Dtos;

namespace TestSystem.Infra.Interfaces;

public interface INotificationRepository
{
    Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(CancellationToken ct, Guid userId, bool? isRead = null);
    Task<NotificationDto> CreateNotificationAsync(CancellationToken ct, NotificationDto notification);
    Task MarkAsReadAsync(CancellationToken ct, Guid notificationId);
    Task MarkAllAsReadAsync(CancellationToken ct, Guid userId);
    Task<int> GetUnreadCountAsync(CancellationToken ct, Guid userId);
    Task BulkCreateNotificationsAsync(CancellationToken ct, IEnumerable<NotificationDto> notifications);
}