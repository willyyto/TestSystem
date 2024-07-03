using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync(CancellationToken ct);
    Task<User> Authenticate(CancellationToken ct, string username, string password);
    Task<User> GetById(CancellationToken ct, Guid id);
    Task<Guid> AddUserAsync(CancellationToken ct, User user);
}