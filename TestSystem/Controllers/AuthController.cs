using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Extensions;
using TestSystem.Filters;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IUserService userService, 
        IUserRepository userRepository,
        ICancellationTokenAccessor cancellationTokenAccessor,
        ILogger<AuthController> logger)
    {
        _userService = userService;
        _userRepository = userRepository;
        _cancellationTokenAccessor = cancellationTokenAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<Guid>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 400)]
    public async Task<IActionResult> Register(RegisterDto request)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            
            var existingUser = await _userRepository.GetByUsernameAsync(ct, request.Username);
            if (existingUser != null)
                return this.BadRequestResponse<string>("Username already exists");

            if (!string.IsNullOrEmpty(request.Email))
            {
                var existingEmail = await _userRepository.GetByEmailAsync(ct, request.Email);
                if (existingEmail != null)
                    return this.BadRequestResponse<string>("Email already exists");
            }

            var userId = await _userService.AddUserAsync(ct, request);
            if (userId == Guid.Empty)
                return this.BadRequestResponse<string>("Failed to create user");

            _logger.LogInformation("User registered successfully: {Username}", request.Username);
            return this.OkResponse(userId, "User registered successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Login user and return JWT tokens
    /// </summary>
    [HttpPost("login")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<TokenDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 401)]
    public async Task<IActionResult> Login(LoginDto request)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            
            if (!await _userService.ValidateUserAsync(ct, request.Username, request.Password))
            {
                _logger.LogWarning("Invalid login attempt for username: {Username}", request.Username);
                return this.UnauthorizedResponse<string>("Invalid credentials");
            }

            var user = await _userRepository.GetByUsernameAsync(ct, request.Username);
            if (user == null || !user.IsActive || user.IsLocked)
            {
                return this.UnauthorizedResponse<string>("Account is not active or locked");
            }

            var token = _userService.GenerateJwtToken(user);
            var refreshToken = _userService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.TokenCreated = DateTime.UtcNow;
            user.TokenExpires = DateTime.UtcNow.AddDays(7);
            user.LastLoginAt = DateTime.UtcNow;
            user.LastLoginIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            await _userRepository.UpdateUserAsync(ct, user);

            var tokenDto = new TokenDto { Token = token, RefreshToken = refreshToken };
            
            _logger.LogInformation("User logged in successfully: {Username}", request.Username);
            return this.OkResponse(tokenDto, "Login successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user login");
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Refresh JWT token using refresh token
    /// </summary>
    [HttpPost("refresh")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<TokenDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 401)]
    public async Task<IActionResult> Refresh([FromBody] TokenDto request)
    {
        try
        {
            var principal = _userService.GetPrincipalFromExpiredToken(request.Token);
            var username = principal.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                return this.UnauthorizedResponse<string>("Invalid token");

            var ct = _cancellationTokenAccessor.Token;
            var user = await _userRepository.GetByUsernameAsync(ct, username);
            
            if (user == null || user.RefreshToken != request.RefreshToken || user.TokenExpires <= DateTime.UtcNow)
                return this.UnauthorizedResponse<string>("Invalid refresh token");

            var newJwtToken = _userService.GenerateJwtToken(user);
            var newRefreshToken = _userService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.TokenCreated = DateTime.UtcNow;
            user.TokenExpires = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateUserAsync(ct, user);

            var tokenDto = new TokenDto { Token = newJwtToken, RefreshToken = newRefreshToken };
            return this.OkResponse(tokenDto, "Token refreshed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Logout user (invalidate refresh token)
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return this.BadRequestResponse<string>("Invalid user");

            var ct = _cancellationTokenAccessor.Token;
            var user = await _userRepository.GetByUsernameAsync(ct, username);
            
            if (user != null)
            {
                user.RefreshToken = string.Empty;
                user.TokenExpires = DateTime.UtcNow;
                await _userRepository.UpdateUserAsync(ct, user);
            }

            return this.OkResponse("Logged out successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return this.ExceptionResponse<string>(ex);
        }
    }
}