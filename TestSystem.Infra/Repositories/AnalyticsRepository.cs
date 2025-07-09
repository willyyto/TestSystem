using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestSystem.Core.Dtos;
using TestSystem.Infra.DataServices;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Infra.Repositories;

[InstanceScopedService]
public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly ILogger<AnalyticsRepository> _logger;
    private readonly ITestSystemDbContextAsync _tsDbContext;

    public AnalyticsRepository(ITestSystemDbContextAsync tsDbContext, ILogger<AnalyticsRepository> logger)
    {
        _tsDbContext = tsDbContext;
        _logger = logger;
    }

    public async Task<TestAnalyticsDto> GetTestAnalyticsAsync(CancellationToken ct, Guid testId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        try
        {
            var test = await _tsDbContext.Tests.FindAsync(testId);
            if (test == null)
                throw new ArgumentException("Test not found");

            var resultsQuery = _tsDbContext.TestResults.Where(tr => tr.TestId == testId);
            var attemptsQuery = _tsDbContext.TestAttempts.Where(ta => ta.TestId == testId);

            if (fromDate.HasValue)
            {
                resultsQuery = resultsQuery.Where(tr => tr.CompletedDate >= fromDate.Value);
                attemptsQuery = attemptsQuery.Where(ta => ta.StartedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                resultsQuery = resultsQuery.Where(tr => tr.CompletedDate <= toDate.Value);
                attemptsQuery = attemptsQuery.Where(ta => ta.StartedAt <= toDate.Value);
            }

            var totalAttempts = await attemptsQuery.CountAsync(ct);
            var completedAttempts = await resultsQuery.CountAsync(ct);
            var passedAttempts = await resultsQuery.CountAsync(tr => tr.Passed, ct);
            
            var averageScore = await resultsQuery
                .AverageAsync(tr => (double?)tr.Score, ct) ?? 0;
                
            var passRate = completedAttempts > 0 ? (double)passedAttempts / completedAttempts : 0;
            
            var averageTime = await resultsQuery
                .AverageAsync(tr => tr.TimeSpent.TotalSeconds, ct);

            var questionAnalytics = await GetQuestionAnalyticsAsync(ct, testId);

            return new TestAnalyticsDto(
                testId,
                test.Name,
                totalAttempts,
                completedAttempts,
                passedAttempts,
                averageScore,
                passRate,
                TimeSpan.FromSeconds(averageTime),
                questionAnalytics.ToList()
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test analytics for test {TestId}", testId);
            throw;
        }
    }

    public async Task<IEnumerable<QuestionAnalyticsDto>> GetQuestionAnalyticsAsync(CancellationToken ct, Guid testId)
    {
        try
        {
            var questions = await _tsDbContext.Questions
                .Include(q => q.Answers)
                .Where(q => q.TestId == testId)
                .ToListAsync(ct);

            var analytics = new List<QuestionAnalyticsDto>();

            foreach (var question in questions)
            {
                var totalResponses = await _tsDbContext.QuestionResults
                    .CountAsync(qr => qr.QuestionId == question.Id, ct);
                    
                var correctResponses = await _tsDbContext.QuestionResults
                    .CountAsync(qr => qr.QuestionId == question.Id && qr.IsCorrect, ct);
                    
                var successRate = totalResponses > 0 ? (double)correctResponses / totalResponses : 0;
                
                var avgTimeSeconds = await _tsDbContext.QuestionResults
                    .Where(qr => qr.QuestionId == question.Id && qr.TimeSpent.HasValue)
                    .AverageAsync(qr => qr.TimeSpent!.Value.TotalSeconds, ct);

                var answerAnalytics = new List<AnswerAnalyticsDto>();
                foreach (var answer in question.Answers)
                {
                    var selectionCount = await _tsDbContext.QuestionResults
                        .CountAsync(qr => qr.QuestionId == question.Id && qr.Answer.Contains(answer.Id.ToString()), ct);
                    
                    var selectionPercentage = totalResponses > 0 ? (double)selectionCount / totalResponses * 100 : 0;

                    answerAnalytics.Add(new AnswerAnalyticsDto(
                        answer.Id,
                        answer.Text,
                        selectionCount,
                        selectionPercentage,
                        answer.IsCorrect
                    ));
                }

                analytics.Add(new QuestionAnalyticsDto(
                    question.Id,
                    question.Text,
                    question.Type.ToString(),
                    totalResponses,
                    correctResponses,
                    successRate,
                    TimeSpan.FromSeconds(avgTimeSeconds),
                    answerAnalytics
                ));
            }

            return analytics;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting question analytics for test {TestId}", testId);
            throw;
        }
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync(CancellationToken ct, Guid? companyId = null)
    {
        try
        {
            var testsQuery = _tsDbContext.Tests.Where(t => !t.IsArchived);
            var usersQuery = _tsDbContext.Users.Where(u => !u.IsArchived);
            var attemptsQuery = _tsDbContext.TestAttempts.AsQueryable();

            if (companyId.HasValue)
            {
                testsQuery = testsQuery.Where(t => t.CompanyId == companyId.Value);
                usersQuery = usersQuery.Where(u => u.CompanyId == companyId.Value);
                attemptsQuery = attemptsQuery.Where(ta => testsQuery.Any(t => t.Id == ta.TestId));
            }

            var totalTests = await testsQuery.CountAsync(ct);
            var activeTests = await testsQuery.CountAsync(t => t.IsActive, ct);
            var totalUsers = await usersQuery.CountAsync(ct);
            var totalAttempts = await attemptsQuery.CountAsync(ct);
            var recentAttempts = await attemptsQuery.CountAsync(ta => ta.StartedAt >= DateTime.UtcNow.AddDays(-7), ct);

            var averageScore = await _tsDbContext.TestResults
                .Where(tr => testsQuery.Any(t => t.Id == tr.TestId))
                .AverageAsync(tr => (double?)tr.Score, ct) ?? 0;

            var recentActivity = await GetRecentActivityAsync(ct, companyId, 10);

            return new DashboardStatsDto(
                totalTests,
                activeTests,
                totalUsers,
                totalAttempts,
                recentAttempts,
                averageScore,
                recentActivity.ToList()
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard stats");
            throw;
        }
    }

    public async Task<IEnumerable<RecentActivityDto>> GetRecentActivityAsync(CancellationToken ct, Guid? companyId = null, int limit = 10)
    {
        try
        {
            var activities = new List<RecentActivityDto>();

            // Get recent test results
            var recentResults = await _tsDbContext.TestResults
                .Include(tr => tr.Test)
                .Where(tr => companyId == null || tr.Test.CompanyId == companyId)
                .OrderByDescending(tr => tr.CompletedDate)
                .Take(limit / 2)
                .Select(tr => new RecentActivityDto(
                    "test_completed",
                    $"Test '{tr.Test.Name}' completed with {tr.Score}% score",
                    tr.CompletedDate,
                    tr.UserId,
                    null
                ))
                .ToListAsync(ct);

            activities.AddRange(recentResults);

            // Get recent test attempts
            var recentAttempts = await _tsDbContext.TestAttempts
                .Include(ta => ta.Test)
                .Where(ta => companyId == null || ta.Test.CompanyId == companyId)
                .Where(ta => !ta.IsCompleted)
                .OrderByDescending(ta => ta.StartedAt)
                .Take(limit / 2)
                .Select(ta => new RecentActivityDto(
                    "test_started",
                    $"Test '{ta.Test.Name}' started",
                    ta.StartedAt,
                    ta.UserId,
                    null
                ))
                .ToListAsync(ct);

            activities.AddRange(recentAttempts);

            return activities.OrderByDescending(a => a.Timestamp).Take(limit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting recent activity");
            throw;
        }
    }

    public async Task<IDictionary<DateTime, int>> GetTestAttemptsOverTimeAsync(CancellationToken ct, Guid testId, int days = 30)
    {
        try
        {
            var startDate = DateTime.UtcNow.AddDays(-days).Date;
            var endDate = DateTime.UtcNow.Date;

            var attempts = await _tsDbContext.TestAttempts
                .Where(ta => ta.TestId == testId && ta.StartedAt >= startDate && ta.StartedAt <= endDate)
                .GroupBy(ta => ta.StartedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            var result = new Dictionary<DateTime, int>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                result[date] = attempts.FirstOrDefault(a => a.Date == date)?.Count ?? 0;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test attempts over time for test {TestId}", testId);
            throw;
        }
    }

    public async Task<IDictionary<string, double>> GetScoreDistributionAsync(CancellationToken ct, Guid testId)
    {
        try
        {
            var scores = await _tsDbContext.TestResults
                .Where(tr => tr.TestId == testId)
                .Select(tr => tr.Score)
                .ToListAsync(ct);

            var distribution = new Dictionary<string, double>
            {
                ["0-20"] = scores.Count(s => s >= 0 && s < 20),
                ["20-40"] = scores.Count(s => s >= 20 && s < 40),
                ["40-60"] = scores.Count(s => s >= 40 && s < 60),
                ["60-80"] = scores.Count(s => s >= 60 && s < 80),
                ["80-100"] = scores.Count(s => s >= 80 && s <= 100)
            };

            var total = scores.Count;
            if (total > 0)
            {
                foreach (var key in distribution.Keys.ToList())
                {
                    distribution[key] = (distribution[key] / total) * 100;
                }
            }

            return distribution;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting score distribution for test {TestId}", testId);
            throw;
        }
    }

    public async Task<IEnumerable<(Guid UserId, string UserName, double AverageScore, int AttemptCount)>> GetTopPerformersAsync(CancellationToken ct, Guid testId, int limit = 10)
    {
        try
        {
            var performers = await _tsDbContext.TestResults
                .Where(tr => tr.TestId == testId)
                .GroupBy(tr => tr.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    AverageScore = g.Average(tr => (double)tr.Score),
                    AttemptCount = g.Count()
                })
                .OrderByDescending(p => p.AverageScore)
                .Take(limit)
                .ToListAsync(ct);

            var userIds = performers.Select(p => p.UserId).ToList();
            var users = await _tsDbContext.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.Name, ct);

            return performers.Select(p => (
                p.UserId,
                users.GetValueOrDefault(p.UserId, "Unknown User"),
                p.AverageScore,
                p.AttemptCount
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting top performers for test {TestId}", testId);
            throw;
        }
    }

    public async Task<double> GetCompanyAverageScoreAsync(CancellationToken ct, Guid companyId, DateTime? fromDate = null)
    {
        try
        {
            var query = _tsDbContext.TestResults
                .Where(tr => tr.Test.CompanyId == companyId);

            if (fromDate.HasValue)
            {
                query = query.Where(tr => tr.CompletedDate >= fromDate.Value);
            }

            return await query.AverageAsync(tr => (double?)tr.Score, ct) ?? 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting company average score for company {CompanyId}", companyId);
            throw;
        }
    }
}