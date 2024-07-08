using System.Security.Claims;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface IUserService
{
    Task<Guid> AddUserAsync(CancellationToken ct, RegisterDto request);
    Task<bool> ValidateUserAsync(CancellationToken ct, string username, string password);
    Task<string> CreateToken(CancellationToken ct, LoginDto request);
    string GenerateJwtToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}