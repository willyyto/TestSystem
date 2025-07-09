using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Extensions;
using TestSystem.Filters;
using TestSystem.Infra.Interfaces;
using TestSystem.Mappers;
using userSystem.Mappers;

namespace TestSystem.Controllers;

[Authorize(Roles = "Administrator")]
[ApiController]
[Route("api/admin/[controller]")]
public class AdminUserController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly ILogger<AdminUserController> _logger;

    public AdminUserController(
        IUserRepository userRepository,
        IUserService userService,
        ICancellationTokenAccessor cancellationTokenAccessor,
        ILogger<AdminUserController> logger)
    {
        _userRepository = userRepository;
        _userService = userService;
        _cancellationTokenAccessor = cancellationTokenAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Get all users with optional filtering
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<PagedResultDto<UserDto>>), 200)]
    public async Task<IActionResult> GetUsers([FromQuery] UserSearchDto searchDto)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var pagedResult = await _userRepository.SearchUsersAsync(ct, searchDto);
            var userDtos = pagedResult.MapToPagedResult(user => user.MapToUserDto());
            
            return this.OkResponse(userDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return this.ExceptionResponse<PagedResultDto<UserDto>>(ex);
        }
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<UserDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> GetUser(Guid id)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var user = await _userRepository.GetByIdAsync(ct, id);
            
            if (user == null)
                return this.NotFoundResponse<string>("User not found");

            return this.OkResponse(user.MapToUserDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user {UserId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<Guid>), 201)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 400)]
    public async Task<IActionResult> CreateUser([FromBody] AddUserDto userDto)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            
            var existingUser = await _userRepository.GetByUsernameAsync(ct, userDto.Username);
            if (existingUser != null)
                return this.BadRequestResponse<string>("Username already exists");

            var existingEmail = await _userRepository.GetByEmailAsync(ct, userDto.Email);
            if (existingEmail != null)
                return this.BadRequestResponse<string>("Email already exists");

            var userId = await _userService.AddUserAsync(ct, userDto);
            
            if (userId == Guid.Empty)
                return this.BadRequestResponse<string>("Failed to create user");

            _logger.LogInformation("User created: {Username}", userDto.Username);
            return this.CreatedResponse(nameof(GetUser), new { id = userId }, userId, "User created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Update user
    /// </summary>
    [HttpPut("{id}")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<UserDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto userDto)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var user = await _userRepository.GetByIdAsync(ct, id);
            
            if (user == null)
                return this.NotFoundResponse<string>("User not found");

            // Update user properties
            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Role = userDto.Role;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Phone = userDto.Phone;
            user.Department = userDto.Department;
            user.JobTitle = userDto.JobTitle;
            user.IsActive = userDto.IsActive;
            user.IsLocked = userDto.IsLocked;
            user.UpdatedOn = DateTime.UtcNow;

            var updatedUser = await _userRepository.UpdateUserAsync(ct, user);
            return this.OkResponse(updatedUser.MapToUserDto(), "User updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {UserId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Delete user (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var deletedUser = await _userRepository.DeleteUserAsync(ct, id);
            
            if (deletedUser == null)
                return this.NotFoundResponse<string>("User not found");

            _logger.LogInformation("User deleted: {UserId}", id);
            return this.OkResponse("User deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user {UserId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Bulk invite users
    /// </summary>
    [HttpPost("bulk-invite")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    public async Task<IActionResult> BulkInviteUsers([FromBody] BulkUserInviteDto inviteDto)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var users = new List<User>();

            foreach (var email in inviteDto.EmailAddresses)
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = email,
                    Email = email,
                    Name = email.Split('@')[0],
                    Role = inviteDto.Role,
                    CompanyId = inviteDto.CompanyId,
                    Password = Guid.NewGuid().ToString("N")[..8], // Temporary password
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow,
                    RefreshToken = string.Empty,
                    TokenCreated = DateTime.UtcNow,
                    TokenExpires = DateTime.UtcNow.AddDays(7)
                };
                users.Add(user);
            }

            await _userRepository.BulkCreateUsersAsync(ct, users);
            
            return this.OkResponse($"Invited {users.Count} users successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk inviting users");
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Bulk update user status
    /// </summary>
    [HttpPut("bulk-status")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    public async Task<IActionResult> BulkUpdateUserStatus([FromBody] BulkUpdateStatusRequest request)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            await _userRepository.BulkUpdateUserStatusAsync(ct, request.UserIds, request.IsActive);
            
            return this.OkResponse($"Updated status for {request.UserIds.Count} users");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk updating user status");
            return this.ExceptionResponse<string>(ex);
        }
    }
}