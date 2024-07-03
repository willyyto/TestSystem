using Microsoft.EntityFrameworkCore;
using TestSystem.Core.Entities;

namespace TestSystem.Infra.DataServices;

public interface ITestSystemDbContextAsync
{
    DbSet<User> Users { get; }
    DbSet<Company> Companies { get; }
    DbSet<Test> Tests { get; }
    DbSet<Question> Questions { get; }
    DbSet<Answer> Answers { get; }


    Task<int> SaveChangesAsync(CancellationToken ct);
}