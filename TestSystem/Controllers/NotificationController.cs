using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Infra.Interfaces;
using TestSystem.Utils;
using TestSystem.Extensions;

namespace TestSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly INotificationRepository _notificationRepository;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(
        INotificationRepository notificationRepository,
        ICancellationTokenAccessor cancellationTokenAccessor,
        ILogger<NotificationController> logger)
    {
        _notificationRepository = notificationRepository;
        _cancellationTokenAccessor = cancellationTokenAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Get user's notifications
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<List<NotificationDto>>), 200)]
    public async Task<IActionResult> GetNotifications([FromQuery] bool? isRead = null)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var userId = UserUtils.GetUserId(User);
            
            var notifications = await _notificationRepository.GetUserNotificationsAsync(ct, userId, isRead);
            return this.OkResponse(notifications.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving notifications");
            return this.ExceptionResponse<List<NotificationDto>>(ex);
        }
    }

    /// <summary>
    /// Mark notification as read
    /// </summary>
    [HttpPut("{id}/read")]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            await _notificationRepository.MarkAsReadAsync(ct, id);
            
            return this.OkResponse("Notification marked as read");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking notification as read {NotificationId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Mark all notifications as read
    /// </summary>
    [HttpPut("read-all")]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    public async Task<IActionResult> MarkAllAsRead()
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var userId = UserUtils.GetUserId(User);
            await _notificationRepository.MarkAllAsReadAsync(ct, userId);
            
            return this.OkResponse("All notifications marked as read");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking all notifications as read");
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Get unread notification count
    /// </summary>
    [HttpGet("unread-count")]
    [ProducesResponseType(typeof(ApiResponseDto<int>), 200)]
    public async Task<IActionResult> GetUnreadCount()
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var userId = UserUtils.GetUserId(User);
            var count = await _notificationRepository.GetUnreadCountAsync(ct, userId);
            
            return this.OkResponse(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unread count");
            return this.ExceptionResponse<int>(ex);
        }
    }
}