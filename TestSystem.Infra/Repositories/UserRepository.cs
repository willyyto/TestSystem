using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestSystem.Core.Entities;
using TestSystem.Infra.DataServices;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Infra.Repositories;

[InstanceScopedService]
public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITestSystemDbContextAsync _tsDbContext;

    public UserRepository(ITestSystemDbContextAsync tsDbContext, ILogger<UserRepository> logger)
    {
        _tsDbContext = tsDbContext;
        _logger = logger;
    }

    public async Task<User> GetById(CancellationToken ct, Guid id)
    {
        return await _tsDbContext.Users.FindAsync(id);
    }

    public async Task<User?> GetByUsername(CancellationToken ct, string username)
    {
        return await _tsDbContext.Users.FirstOrDefaultAsync(u => u.Username == username, ct);
    }

    public async Task<List<User>> GetAllUsersAsync(CancellationToken ct)
    {
        var users = await _tsDbContext.Users.Include(u => u.Company).ToListAsync(ct);
        return users;
    }

    public async Task<Guid> AddUserAsync(CancellationToken ct, User user)
    {
        try
        {
            await _tsDbContext.Users.AddAsync(user, ct);
            await _tsDbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            return Guid.Empty;
        }

        return user.Id;
    }

    public async Task UpdateUserAsync(CancellationToken ct, User user)
    {
        _tsDbContext.Users.Update(user);
        await _tsDbContext.SaveChangesAsync(ct);
    }


    public async Task<User?> DeleteUserAsync(CancellationToken ct, Guid id)
    {
        var user = await _tsDbContext.Users
            .Include(t => t.Company)
            .FirstOrDefaultAsync(t => t.Id == id, ct);

        if (user == null) return null;

        var testresults = await _tsDbContext.TestResults
            .Include(t => t.QuestionResults)
            .Where(t => t.IsActive == true && t.IsArchived == false)
            .FirstOrDefaultAsync(t => t.UserId == id, ct);

        if (testresults != null) _tsDbContext.TestResults.RemoveRange(testresults);
        _tsDbContext.Users.Remove(user);
        await _tsDbContext.SaveChangesAsync(ct);

        return user;
    }
}