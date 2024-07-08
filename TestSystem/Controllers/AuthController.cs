using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;

    public AuthController(IUserService userService, IUserRepository userRepository,
        ICancellationTokenAccessor cancellationTokenAccessor)
    {
        _userService = userService;
        _userRepository = userRepository;
        _cancellationTokenAccessor = cancellationTokenAccessor;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto request)
    {
        var ct = _cancellationTokenAccessor.Token;
        var userId = await _userService.AddUserAsync(ct, request);
        if (userId == null || userId == Guid.Empty)
            return NoContent();
        return Ok(userId);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto request)
    {
        var ct = _cancellationTokenAccessor.Token;
        if (!await _userService.ValidateUserAsync(ct, request.Username, request.Password)) return Unauthorized();

        var user = await _userRepository.GetByUsername(ct, request.Username);
        
        var token = _userService.GenerateJwtToken(user);
        var refreshToken = _userService.GenerateRefreshToken();
        
        await _userRepository.UpdateUserAsync(ct, user);
        return Ok(new TokenDto { Token = token, RefreshToken = refreshToken });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenDto request)
    {
        var principal = _userService.GetPrincipalFromExpiredToken(request.Token);
        var username = principal.Identity.Name;

        var ct = _cancellationTokenAccessor.Token;
        var user = await _userRepository.GetByUsername(ct, username);
        if (user == null || user.RefreshToken != request.RefreshToken || user.TokenExpires <= DateTime.UtcNow)
            return Unauthorized();

        var newJwtToken = _userService.GenerateJwtToken(user);
        var newRefreshToken = _userService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.TokenCreated = DateTime.UtcNow;
        user.TokenExpires = DateTime.UtcNow.AddDays(7);
        await _userRepository.UpdateUserAsync(ct, user);

        return Ok(new TokenDto {Token = newJwtToken, RefreshToken = newRefreshToken});
    }
}