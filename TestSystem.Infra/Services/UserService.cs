using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Infra.Services;

[InstanceScopedService]
public class UserService : IUserService
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    public UserService(IConfiguration configuration, IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> AddUserAsync(CancellationToken ct, RegisterDto request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Password = request.Password,
            Name = request.Name,
            Email = request.Email,
            Role = request.Role,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            RefreshToken = "",
            TokenExpires = DateTime.UtcNow,
            TokenCreated = DateTime.UtcNow
        };
        user.Password = _passwordHasher.HashPassword(user, user.Password);
        var userId = await _userRepository.AddUserAsync(ct, user);
        return userId;
    }

    public async Task<bool> ValidateUserAsync(CancellationToken ct, string username, string password)
    {
        var user = await _userRepository.GetByUsername(ct, username);
        if (user == null) return false;

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        return result == PasswordVerificationResult.Success;
    }

    public async Task<string> CreateToken(CancellationToken ct, LoginDto request)
    {
        var user = await _userRepository.GetByUsername(ct, request.Username);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Token"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }
}