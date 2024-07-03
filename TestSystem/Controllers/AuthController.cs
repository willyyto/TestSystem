using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;

    public AuthController(IUserRepository userRepository, IConfiguration config,
        ICancellationTokenAccessor cancellationTokenAccessor)
    {
        _userRepository = userRepository;
        _config = config;
        _cancellationTokenAccessor = cancellationTokenAccessor;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var ct = _cancellationTokenAccessor.Token;
        var user = await _userRepository.Authenticate(ct, login.Username, login.Password);

        if (user == null)
            return Unauthorized();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role) // Add role claim
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new {Token = tokenString});
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto register)
    {
        var ct = _cancellationTokenAccessor.Token;
        var user = new User
        {
            Name = register.Name,
            Username = register.Username,
            Email = register.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
            Role = register.Role // Assign role
        };
        var id = await _userRepository.AddUserAsync(ct, user);
        if (id == Guid.Empty)
            return BadRequest();
        return Ok();
    }
}