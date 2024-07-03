using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestSystem.Core.Entities;
using TestSystem.Infra;
using TestSystem.Infra.DataServices;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Infra.Repositories;

[InstanceScopedService]
public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly ITestSystemDbContextAsync _msDbContext;

    public UserRepository(ITestSystemDbContextAsync msDbContext, ILogger<UserRepository> logger)
    {
        _msDbContext = msDbContext;
        _logger = logger;
    }

    public async Task<List<User>> GetAllUsersAsync(CancellationToken ct)
    {
        var Users = await _msDbContext.Users.ToListAsync(ct);
        return Users;
    }

    public async Task<Guid> AddUserAsync(CancellationToken ct, User User)
    {
        try
        {
            await _msDbContext.Users.AddAsync(User, ct);
            await _msDbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            return Guid.Empty;
        }

        return User.Id;
    }
}