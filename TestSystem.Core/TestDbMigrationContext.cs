using Microsoft.EntityFrameworkCore;
using TestSystem.Core.Entities;

namespace TestSystem.Core;

public class TestDbMigrationContext : DbContext
{
    public TestDbMigrationContext(DbContextOptions opts) : base(opts)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        new UserConfig().Configure(builder.Entity<User>());
    }
}