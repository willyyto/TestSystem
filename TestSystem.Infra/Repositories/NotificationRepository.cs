using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Infra.DataServices;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Infra.Repositories;

[InstanceScopedService]
public class NotificationRepository : INotificationRepository
{
    private readonly ILogger<NotificationRepository> _logger;
    private readonly ITestSystemDbContextAsync _tsDbContext;

    public NotificationRepository(ITestSystemDbContextAsync tsDbContext, ILogger<NotificationRepository> logger)
    {
        _tsDbContext = tsDbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(CancellationToken ct, Guid userId, bool? isRead = null)
    {
        try
        {
            var query = _tsDbContext.Notifications.Where(n => n.UserId == userId);
            
            if (isRead.HasValue)
            {
                query = query.Where(n => n.IsRead == isRead.Value);
            }

            var notifications = await query
                .OrderByDescending(n => n.CreatedOn)
                .Take(50) // Limit to 50 most recent notifications
                .ToListAsync(ct);

            return notifications.Select(n => new NotificationDto(
                n.Id,
                n.UserId,
                n.Type,
                n.Title,
                n.Message,
                n.IsRead,
                n.CreatedOn,
                n.ActionUrl
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving notifications for user {UserId}", userId);
            throw;
        }
    }

    public async Task<NotificationDto> CreateNotificationAsync(CancellationToken ct, NotificationDto notification)
    {
        try
        {
            var entity = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = notification.UserId,
                Type = notification.Type,
                Title = notification.Title,
                Message = notification.Message,
                IsRead = false,
                CreatedOn = DateTime.UtcNow,
                ActionUrl = notification.ActionUrl,
                IsActive = true,
                IsArchived = false,
                UpdatedOn = DateTime.UtcNow
            };

            await _tsDbContext.Notifications.AddAsync(entity, ct);
            await _tsDbContext.SaveChangesAsync(ct);

            _logger.LogInformation("Created notification {NotificationId} for user {UserId}", entity.Id, entity.UserId);

            return new NotificationDto(
                entity.Id,
                entity.UserId,
                entity.Type,
                entity.Title,
                entity.Message,
                entity.IsRead,
                entity.CreatedOn,
                entity.ActionUrl
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating notification for user {UserId}", notification.UserId);
            throw;
        }
    }

    public async Task MarkAsReadAsync(CancellationToken ct, Guid notificationId)
    {
        try
        {
            var notification = await _tsDbContext.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                notification.UpdatedOn = DateTime.UtcNow;
                await _tsDbContext.SaveChangesAsync(ct);
                
                _logger.LogInformation("Marked notification {NotificationId} as read", notificationId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking notification {NotificationId} as read", notificationId);
            throw;
        }
    }

    public async Task MarkAllAsReadAsync(CancellationToken ct, Guid userId)
    {
        try
        {
            var notifications = await _tsDbContext.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync(ct);

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.UpdatedOn = DateTime.UtcNow;
            }

            await _tsDbContext.SaveChangesAsync(ct);
            
            _logger.LogInformation("Marked {Count} notifications as read for user {UserId}", notifications.Count, userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking all notifications as read for user {UserId}", userId);
            throw;
        }
    }

    public async Task<int> GetUnreadCountAsync(CancellationToken ct, Guid userId)
    {
        try
        {
            return await _tsDbContext.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unread count for user {UserId}", userId);
            throw;
        }
    }

    public async Task BulkCreateNotificationsAsync(CancellationToken ct, IEnumerable<NotificationDto> notifications)
    {
        try
        {
            var entities = notifications.Select(n => new Notification
            {
                Id = Guid.NewGuid(),
                UserId = n.UserId,
                Type = n.Type,
                Title = n.Title,
                Message = n.Message,
                IsRead = false,
                CreatedOn = DateTime.UtcNow,
                ActionUrl = n.ActionUrl,
                IsActive = true,
                IsArchived = false,
                UpdatedOn = DateTime.UtcNow
            }).ToList();

            await _tsDbContext.Notifications.AddRangeAsync(entities, ct);
            await _tsDbContext.SaveChangesAsync(ct);

            _logger.LogInformation("Bulk created {Count} notifications", entities.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk creating notifications");
            throw;
        }
    }
}