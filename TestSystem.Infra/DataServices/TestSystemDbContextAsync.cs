using Microsoft.EntityFrameworkCore;
using TestSystem.Core.Entities;

namespace TestSystem.Infra.DataServices;

public class TestSystemDbContextAsync : AsyncDbContext, ITestSystemDbContextAsync
{
    public TestSystemDbContextAsync(DbContextOptions<TestSystemDbContextAsync> options) : base(options)
    {
    }

    public TestSystemDbContextAsync(DbContextOptions options) : base(options)
    {
    }

    // Enhanced DbSets with all new entities
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
        // Configure all entities with their respective configurations
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
        
        ConfigureEnumConversions(builder);
        
        // Configure global query filters if needed
        ConfigureGlobalFilters(builder);

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

    private void ConfigureGlobalFilters(ModelBuilder builder)
    {
        // Global filters for soft deletes - commented out by default
        // Uncomment these if you want automatic filtering of archived items
        
        // builder.Entity<Test>().HasQueryFilter(t => !t.IsArchived);
        // builder.Entity<Question>().HasQueryFilter(q => !q.IsArchived);
        // builder.Entity<Answer>().HasQueryFilter(a => !a.IsArchived);
        // builder.Entity<User>().HasQueryFilter(u => !u.IsArchived);
        // builder.Entity<Company>().HasQueryFilter(c => !c.IsArchived);
        // builder.Entity<TestResult>().HasQueryFilter(tr => !tr.IsArchived);
        // builder.Entity<QuestionResult>().HasQueryFilter(qr => !qr.IsArchived);
        // builder.Entity<MatchPair>().HasQueryFilter(mp => !mp.IsArchived);
        // builder.Entity<OrderingItem>().HasQueryFilter(oi => !oi.IsArchived);
        // builder.Entity<TestAttempt>().HasQueryFilter(ta => !ta.IsArchived);
        // builder.Entity<TestSchedule>().HasQueryFilter(ts => !ts.IsArchived);
    }
}