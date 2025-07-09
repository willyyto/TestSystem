using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Extensions;
using TestSystem.Filters;
using TestSystem.Infra.Interfaces;
using TestSystem.Utils;
using userSystem.Mappers;

namespace TestSystem.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;

    public UserController(
        IUserRepository userRepository,
        ICancellationTokenAccessor cancellationTokenAccessor, 
        IUserService userService)
    {
        _userRepository = userRepository;
        _cancellationTokenAccessor = cancellationTokenAccessor;
        _userService = userService;
    }

    /// <summary>
    /// Get current user's profile
    /// </summary>
    [HttpGet("profile")]
    [ProducesResponseType(typeof(ApiResponseDto<UserDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var userId = UserUtils.GetUserId(User);
            var user = await _userRepository.GetByIdAsync(ct, userId);

            if (user == null) 
                return this.NotFoundResponse<string>("User not found");

            return this.OkResponse(user.MapToUserDto());
        }
        catch (UnauthorizedAccessException)
        {
            return this.UnauthorizedResponse<string>("Access denied");
        }
        catch (Exception ex)
        {
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Update current user's profile
    /// </summary>
    [HttpPut("profile")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<UserDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto profileDto)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var userId = UserUtils.GetUserId(User);
            var user = await _userRepository.GetByIdAsync(ct, userId);

            if (user == null)
                return this.NotFoundResponse<string>("User not found");

            // Update allowed profile fields
            user.Name = profileDto.Name;
            user.FirstName = profileDto.FirstName;
            user.LastName = profileDto.LastName;
            user.Phone = profileDto.Phone;
            user.Timezone = profileDto.Timezone;
            user.Language = profileDto.Language;
            user.NotificationEmailEnabled = profileDto.NotificationEmailEnabled;
            user.NotificationSmsEnabled = profileDto.NotificationSmsEnabled;
            user.UpdatedOn = DateTime.UtcNow;

            var updatedUser = await _userRepository.UpdateUserAsync(ct, user);
            return this.OkResponse(updatedUser.MapToUserDto(), "Profile updated successfully");
        }
        catch (UnauthorizedAccessException)
        {
            return this.UnauthorizedResponse<string>("Access denied");
        }
        catch (Exception ex)
        {
            return this.ExceptionResponse<string>(ex);
        }
    }
}