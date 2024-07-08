using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync(CancellationToken ct);
    Task<User> GetById(CancellationToken ct, Guid id);
    Task<Guid> AddUserAsync(CancellationToken ct, User user);
    Task<User?> GetByUsername(CancellationToken ct, string username);
    Task UpdateUserAsync(CancellationToken ct, User user);
}