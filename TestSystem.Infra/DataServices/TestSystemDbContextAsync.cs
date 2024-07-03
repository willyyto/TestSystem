using Microsoft.EntityFrameworkCore;
using TestSystem.Core.Entities;

namespace TestSystem.Infra.DataServices;

public class TestSystemDbContextAsync : AsyncDbContext, ITestSystemDbContextAsync
{
    public TestSystemDbContextAsync(DbContextOptions<TestSystemDbContextAsync> options) :
        base(options)
    {
    }

    public TestSystemDbContextAsync(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        new UserConfig().Configure(builder.Entity<User>());
    }
}