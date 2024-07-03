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
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Test> Tests => Set<Test>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        new UserConfig().Configure(builder.Entity<User>());
        new CompanyConfig().Configure(builder.Entity<Company>());
        new TestConfig().Configure(builder.Entity<Test>());
        new QuestionConfig().Configure(builder.Entity<Question>());
        new AnswerConfig().Configure(builder.Entity<Answer>());
    }
}