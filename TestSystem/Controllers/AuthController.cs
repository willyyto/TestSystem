using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Infra.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TestSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController: ControllerBase
{
    public static User user = new User();
    private readonly IUserService _userService;

    public AuthController( IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public ActionResult<User> Register(UserDto request)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.Username = request.Username;
        user.Password = passwordHash;

        return Ok(user);
    }
    
    [HttpPost("login")]
    public ActionResult<User> Login(UserDto request)
    {
        if (user.Username != request.Username)
            return BadRequest("User Not Found");
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return BadRequest("Wrong password.");
        
        string token = _userService.CreateToken(user);
        var refreshToken = _userService.GenerateRefreshToken();
        SetRefreshToken(refreshToken);
        
        return Ok(token);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (!user.RefreshToken.Equals(refreshToken))
        {
            return Unauthorized("Invalid refresh token");
        }
        else if (user.TokenExpires < DateTime.Now)
        {
            return Unauthorized("Token expired");
        }

        string token = _userService.CreateToken(user);
        var newRefreshToken = _userService.GenerateRefreshToken();
        SetRefreshToken(newRefreshToken);
        return Ok(token);
    }
    private void SetRefreshToken(RefreshToken newRefreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
        Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpires = newRefreshToken.Expires;
    }
}