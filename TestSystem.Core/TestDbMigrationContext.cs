using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestSystem.Core.Entities;

namespace TestSystem.Core;

public class TestDbMigrationContext : DbContext
{
    public TestDbMigrationContext(DbContextOptions<TestDbMigrationContext> opts) : base(opts)
    {
    }

    // DbSets for all entities - required for migrations
    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Test> Tests => Set<Test>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<TestResult> TestResults => Set<TestResult>();
    public DbSet<QuestionResult> QuestionResults => Set<QuestionResult>();
    public DbSet<MatchPair> MatchPairs => Set<MatchPair>();
    public DbSet<OrderingItem> OrderingItems => Set<OrderingItem>();
    public DbSet<TestAttempt> TestAttempts => Set<TestAttempt>();
    public DbSet<TestSchedule> TestSchedules => Set<TestSchedule>();
    public DbSet<Notification> Notifications => Set<Notification>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        Console.WriteLine("Configuring entity models for migration context...");

        // Apply all entity configurations
        new UserConfig().Configure(builder.Entity<User>());
        new CompanyConfig().Configure(builder.Entity<Company>());
        new TestConfig().Configure(builder.Entity<Test>());
        new QuestionConfig().Configure(builder.Entity<Question>());
        new AnswerConfig().Configure(builder.Entity<Answer>());
        new TestResultConfig().Configure(builder.Entity<TestResult>());
        new QuestionResultConfig().Configure(builder.Entity<QuestionResult>());
        new MatchPairConfig().Configure(builder.Entity<MatchPair>());
        new OrderingItemConfig().Configure(builder.Entity<OrderingItem>());
        new TestAttemptConfig().Configure(builder.Entity<TestAttempt>());
        new TestScheduleConfig().Configure(builder.Entity<TestSchedule>());
        new NotificationConfig().Configure(builder.Entity<Notification>());
        
        // Configure custom value converters for enums
        ConfigureEnumConversions(builder);

        Console.WriteLine("Entity model configuration completed.");

        base.OnModelCreating(builder);
    }

    private void ConfigureEnumConversions(ModelBuilder builder)
    {
        // Configure enum to string conversions for better database readability
        builder.Entity<Question>()
            .Property(q => q.Type)
            .HasConversion<string>();

        builder.Entity<Test>()
            .Property(t => t.Visibility)
            .HasConversion<string>();

        builder.Entity<Test>()
            .Property(t => t.TestType)
            .HasConversion<string>();

        builder.Entity<Test>()
            .Property(t => t.Feedback)
            .HasConversion<string>();

        builder.Entity<Test>()
            .Property(t => t.TestAccessControl)
            .HasConversion<string>();

        builder.Entity<Test>()
            .Property(t => t.GradingScheme)
            .HasConversion<string>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Fallback configuration if not configured externally
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("TestManagementDbConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        base.OnConfiguring(optionsBuilder);
    }
}
