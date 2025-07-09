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
    DbSet<TestResult> TestResults { get; }
    DbSet<QuestionResult> QuestionResults { get; }
    DbSet<MatchPair> MatchPairs { get; }
    DbSet<OrderingItem> OrderingItems { get; }
    DbSet<TestAttempt> TestAttempts { get; }
    DbSet<TestSchedule> TestSchedules { get; }
    DbSet<Notification> Notifications { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}