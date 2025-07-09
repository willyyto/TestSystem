using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
    private readonly ICompanyRepository _companyRepository;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _userRepository;

    public UserService(IConfiguration configuration, IUserRepository userRepository,
        ICompanyRepository companyRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _companyRepository = companyRepository;
    }

    public async Task<Guid> AddUserAsync(CancellationToken ct, RegisterDto request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Password = string.Empty, // Will be set below
            Name = request.Name,
            Email = request.Email,
            Role = request.Role,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            RefreshToken = string.Empty,
            TokenCreated = DateTime.UtcNow,
            TokenExpires = DateTime.UtcNow.AddDays(7)
        };
        
        // Hash the password using Identity's password hasher
        user.Password = _passwordHasher.HashPassword(user, request.Password);
        
        var userId = await _userRepository.CreateUserAsync(ct, user);
        return userId;
    }

    public async Task<Guid> AddUserAsync(CancellationToken ct, AddUserDto request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Password = string.Empty, // Will be set below
            CompanyId = request.CompanyId,
            Name = request.Name,
            Email = request.Email,
            Role = request.Role,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            RefreshToken = string.Empty,
            TokenExpires = DateTime.UtcNow.AddDays(7),
            TokenCreated = DateTime.UtcNow
        };
        
        // Hash the password using Identity's password hasher
        user.Password = _passwordHasher.HashPassword(user, request.Password);
        
        var userId = await _userRepository.CreateUserAsync(ct, user);
        return userId;
    }

    public async Task<bool> ValidateUserAsync(CancellationToken ct, string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(ct, username);
        if (user == null || !user.IsActive || user.IsLocked) 
            return false;

        // Use Identity's password verification
        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
    }

    public async Task<string> CreateToken(CancellationToken ct, LoginDto request)
    {
        var user = await _userRepository.GetByUsernameAsync(ct, request.Username);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Token"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("name", user.Name)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Token"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("companyId", user.CompanyId?.ToString() ?? string.Empty)
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Token"])),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase)) 
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}