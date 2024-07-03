using Microsoft.EntityFrameworkCore;
using TestSystem.Core.Entities;

namespace TestSystem.Infra.DataServices;

public interface ITestSystemDbContextAsync
{
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}