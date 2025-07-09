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

        // Add performance indexes
        ConfigureIndexes(builder);
        
        // Configure global query filters if needed
        ConfigureGlobalFilters(builder);

        base.OnModelCreating(builder);
    }

    private void ConfigureIndexes(ModelBuilder builder)
    {
        // Performance indexes for frequently queried combinations
        builder.Entity<TestResult>()
            .HasIndex(tr => new { tr.TestId, tr.UserId, tr.CompletedDate })
            .HasDatabaseName("IX_TestResult_Performance");

        builder.Entity<QuestionResult>()
            .HasIndex(qr => new { qr.TestResultId, qr.QuestionId })
            .HasDatabaseName("IX_QuestionResult_Performance");

        builder.Entity<TestAttempt>()
            .HasIndex(ta => new { ta.TestId, ta.UserId, ta.StartedAt })
            .HasDatabaseName("IX_TestAttempt_Performance");

        builder.Entity<TestAttempt>()
            .HasIndex(ta => new { ta.TestId, ta.UserId, ta.AttemptNumber })
            .HasDatabaseName("IX_TestAttempt_Unique")
            .IsUnique();

        // Search indexes
        builder.Entity<Test>()
            .HasIndex(t => new { t.CompanyId, t.IsActive, t.IsArchived })
            .HasDatabaseName("IX_Test_Search");

        builder.Entity<User>()
            .HasIndex(u => new { u.CompanyId, u.IsActive, u.IsArchived })
            .HasDatabaseName("IX_User_Search");

        builder.Entity<Question>()
            .HasIndex(q => new { q.TestId, q.DisplayOrder })
            .HasDatabaseName("IX_Question_Order");

        // Unique constraints
        builder.Entity<User>()
            .HasIndex(u => u.Username)
            .HasDatabaseName("IX_User_Username")
            .IsUnique();

        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .HasDatabaseName("IX_User_Email")
            .IsUnique();

        builder.Entity<Company>()
            .HasIndex(c => c.CustomDomain)
            .HasDatabaseName("IX_Company_CustomDomain")
            .IsUnique()
            .HasFilter("[CustomDomain] IS NOT NULL");

        // Frequently filtered columns
        builder.Entity<Test>()
            .HasIndex(t => t.IsPublic)
            .HasDatabaseName("IX_Test_IsPublic");

        builder.Entity<Test>()
            .HasIndex(t => t.InviteCode)
            .HasDatabaseName("IX_Test_InviteCode")
            .HasFilter("[InviteCode] IS NOT NULL");

        builder.Entity<TestResult>()
            .HasIndex(tr => tr.Passed)
            .HasDatabaseName("IX_TestResult_Passed");

        builder.Entity<QuestionResult>()
            .HasIndex(qr => qr.RequiresManualGrading)
            .HasDatabaseName("IX_QuestionResult_ManualGrading");
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