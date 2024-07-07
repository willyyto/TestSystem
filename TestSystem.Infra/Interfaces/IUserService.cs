using TestSystem.Core.Dtos;

namespace TestSystem.Infra.Interfaces;

public interface IUserService
{
    Task<Guid> AddUserAsync(CancellationToken ct, RegisterDto request);
    Task<bool> ValidateUserAsync(CancellationToken ct, string username, string password);
    Task<string> CreateToken(CancellationToken ct, LoginDto request);
}