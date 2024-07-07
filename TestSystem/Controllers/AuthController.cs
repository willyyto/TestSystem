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

        var tokenString = await _userService.CreateToken(ct, request);

        return Ok(new {Token = tokenString});
    }
}