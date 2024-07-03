using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync(CancellationToken ct);
    Task<Guid> AddUserAsync(CancellationToken ct, User user);
}