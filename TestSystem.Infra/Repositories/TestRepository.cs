using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Infra.DataServices;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Infra.Repositories;

[InstanceScopedService]
public class TestRepository : ITestRepository
{
    private readonly ILogger<TestRepository> _logger;
    private readonly ITestSystemDbContextAsync _tsDbContext;

    public TestRepository(ITestSystemDbContextAsync tsDbContext, ILogger<TestRepository> logger)
    {
        _tsDbContext = tsDbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<Test>> GetTestsAsync(CancellationToken ct)
    {
        return await _tsDbContext.Tests
            .Include(t => t.Company)
            .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
            .Include(t => t.Questions)
                .ThenInclude(q => q.MatchPairs)
            .Include(t => t.Questions)
                .ThenInclude(q => q.OrderingItems)
            .Where(t => t.IsActive && !t.IsArchived)
            .OrderByDescending(t => t.CreatedOn)
            .ToListAsync(ct);
    }

    public async Task<Test?> GetTestByIdAsync(CancellationToken ct, Guid id)
    {
        return await _tsDbContext.Tests
            .Include(t => t.Company)
            .Include(t => t.Questions.OrderBy(q => q.DisplayOrder))
                .ThenInclude(q => q.Answers)
            .Include(t => t.Questions)
                .ThenInclude(q => q.MatchPairs)
            .Include(t => t.Questions)
                .ThenInclude(q => q.OrderingItems.OrderBy(oi => oi.CorrectOrder))
            .Include(t => t.Schedules)
            .FirstOrDefaultAsync(t => t.Id == id && t.IsActive && !t.IsArchived, ct);
    }

    public async Task<Test?> GetTestForTakingAsync(CancellationToken ct, Guid id, string? password = null)
    {
        var test = await GetTestByIdAsync(ct, id);
        
        if (test == null) return null;
        
        // Validate test availability
        var now = DateTime.UtcNow;
        if (test.AvailableFrom.HasValue && now < test.AvailableFrom.Value) return null;
        if (test.AvailableUntil.HasValue && now > test.AvailableUntil.Value) return null;
        
        // Validate password if required
        if (test.RequirePassword && !string.IsNullOrEmpty(test.Password))
        {
            if (password != test.Password) return null;
        }
        
        return test;
    }

    public async Task<bool> CanUserTakeTestAsync(CancellationToken ct, Guid testId, Guid userId)
    {
        var test = await _tsDbContext.Tests.FindAsync(testId);
        if (test == null || !test.IsActive || test.IsArchived) return false;
        
        var attemptCount = await GetUserAttemptCountAsync(ct, testId, userId);
        return attemptCount < test.MaximumAttempts;
    }

    public async Task<int> GetUserAttemptCountAsync(CancellationToken ct, Guid testId, Guid userId)
    {
        return await _tsDbContext.TestAttempts
            .CountAsync(ta => ta.TestId == testId && ta.UserId == userId && ta.IsCompleted, ct);
    }

    public async Task<Test> CreateTestAsync(CancellationToken ct, Test test)
    {
        _tsDbContext.Tests.Add(test);
        await _tsDbContext.SaveChangesAsync(ct);
        return test;
    }

    public async Task<Test> UpdateTestAsync(CancellationToken ct, Test test)
    {
        _tsDbContext.Tests.Update(test);
        await _tsDbContext.SaveChangesAsync(ct);
        return test;
    }

    public async Task<Test?> DeleteTestByIdAsync(CancellationToken ct, Guid id)
    {
        var test = await _tsDbContext.Tests
            .Include(t => t.TestResults)
                .ThenInclude(tr => tr.QuestionResults)
            .Include(t => t.TestAttempts)
            .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
            .Include(t => t.Questions)
                .ThenInclude(q => q.MatchPairs)
            .Include(t => t.Questions)
                .ThenInclude(q => q.OrderingItems)
            .FirstOrDefaultAsync(t => t.Id == id, ct);

        if (test == null) return null;

        // Soft delete by marking as archived
        test.IsArchived = true;
        test.IsActive = false;
        
        await _tsDbContext.SaveChangesAsync(ct);
        return test;
    }

    public async Task<PagedResultDto<Test>> SearchTestsAsync(CancellationToken ct, TestSearchDto searchDto)
    {
        var query = _tsDbContext.Tests
            .Include(t => t.Company)
            .Where(t => !t.IsArchived);

        // Apply filters
        if (!string.IsNullOrEmpty(searchDto.SearchTerm))
        {
            query = query.Where(t => t.Name.Contains(searchDto.SearchTerm) || 
                                   t.Description.Contains(searchDto.SearchTerm));
        }

        if (searchDto.CompanyId.HasValue)
        {
            query = query.Where(t => t.CompanyId == searchDto.CompanyId.Value);
        }

        if (searchDto.TestTypes?.Any() == true)
        {
            var testTypes = searchDto.TestTypes.Select(tt => Enum.Parse<TestType>(tt));
            query = query.Where(t => testTypes.Contains(t.TestType));
        }

        if (searchDto.CreatedAfter.HasValue)
        {
            query = query.Where(t => t.CreatedOn >= searchDto.CreatedAfter.Value);
        }

        if (searchDto.CreatedBefore.HasValue)
        {
            query = query.Where(t => t.CreatedOn <= searchDto.CreatedBefore.Value);
        }

        // Apply sorting
        query = searchDto.SortBy?.ToLower() switch
        {
            "name" => searchDto.SortDirection?.ToLower() == "desc" 
                ? query.OrderByDescending(t => t.Name)
                : query.OrderBy(t => t.Name),
            "createdon" => searchDto.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(t => t.CreatedOn)
                : query.OrderBy(t => t.CreatedOn),
            "startdate" => searchDto.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(t => t.StartDate)
                : query.OrderBy(t => t.StartDate),
            _ => query.OrderByDescending(t => t.CreatedOn)
        };

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((searchDto.Page - 1) * searchDto.PageSize)
            .Take(searchDto.PageSize)
            .ToListAsync(ct);

        return new PagedResultDto<Test>(
            items,
            totalCount,
            searchDto.Page,
            searchDto.PageSize,
            (int)Math.Ceiling((double)totalCount / searchDto.PageSize)
        );
    }

    // Additional repository methods would continue here...
    // Implementation for remaining interface methods follows similar patterns

    public async Task<TestAttempt> CreateTestAttemptAsync(CancellationToken ct, TestAttempt attempt)
    {
        _tsDbContext.TestAttempts.Add(attempt);
        await _tsDbContext.SaveChangesAsync(ct);
        return attempt;
    }

    public async Task<TestAttempt> UpdateTestAttemptAsync(CancellationToken ct, TestAttempt attempt)
    {
        _tsDbContext.TestAttempts.Update(attempt);
        await _tsDbContext.SaveChangesAsync(ct);
        return attempt;
    }

    public async Task<TestAttempt?> GetActiveTestAttemptAsync(CancellationToken ct, Guid testId, Guid userId)
    {
        return await _tsDbContext.TestAttempts
            .FirstOrDefaultAsync(ta => ta.TestId == testId && 
                                     ta.UserId == userId && 
                                     !ta.IsCompleted && 
                                     !ta.IsAbandoned, ct);
    }

    public async Task<IEnumerable<TestAttempt>> GetTestAttemptsAsync(CancellationToken ct, Guid testId, Guid? userId = null)
    {
        var query = _tsDbContext.TestAttempts.Where(ta => ta.TestId == testId);
        
        if (userId.HasValue)
            query = query.Where(ta => ta.UserId == userId.Value);
            
        return await query.OrderByDescending(ta => ta.StartedAt).ToListAsync(ct);
    }

    // Implement remaining methods...
    public async Task<IEnumerable<TestResult>> GetTestResultsAsync(CancellationToken ct, Guid? testId = null, Guid? userId = null)
    {
        var query = _tsDbContext.TestResults
            .Include(tr => tr.Test)
                .ThenInclude(t => t.Company)
            .Include(tr => tr.QuestionResults)
                .ThenInclude(qr => qr.Question)
            .AsQueryable();

        if (testId.HasValue)
            query = query.Where(tr => tr.TestId == testId.Value);
            
        if (userId.HasValue)
            query = query.Where(tr => tr.UserId == userId.Value);

        return await query.OrderByDescending(tr => tr.CompletedDate).ToListAsync(ct);
    }

    public async Task<TestResult?> GetTestResultByIdAsync(CancellationToken ct, Guid id)
    {
        return await _tsDbContext.TestResults
            .Include(tr => tr.Test)
                .ThenInclude(t => t.Company)
            .Include(tr => tr.QuestionResults)
                .ThenInclude(qr => qr.Question)
                    .ThenInclude(q => q.Answers)
            .Include(tr => tr.TestAttempt)
            .FirstOrDefaultAsync(tr => tr.Id == id, ct);
    }

    public async Task<IEnumerable<TestResult>> GetTestResultsByUserIdAsync(CancellationToken ct, Guid userId)
    {
        return await GetTestResultsAsync(ct, null, userId);
    }

    public async Task<TestResult?> GetTestResultByIdAndUserIdAsync(CancellationToken ct, Guid id, Guid userId)
    {
        return await _tsDbContext.TestResults
            .Include(tr => tr.Test)
                .ThenInclude(t => t.Company)
            .Include(tr => tr.QuestionResults)
                .ThenInclude(qr => qr.Question)
                    .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(tr => tr.Id == id && tr.UserId == userId, ct);
    }

    public async Task<TestResult> CreateTestResultAsync(CancellationToken ct, TestResult testResult)
    {
        _tsDbContext.TestResults.Add(testResult);
        await _tsDbContext.SaveChangesAsync(ct);
        return testResult;
    }

    public async Task<TestResult> UpdateTestResultAsync(CancellationToken ct, TestResult testResult)
    {
        _tsDbContext.TestResults.Update(testResult);
        await _tsDbContext.SaveChangesAsync(ct);
        return testResult;
    }

    public async Task<IEnumerable<Test>> GetTestsByCompanyAsync(CancellationToken ct, Guid companyId)
    {
        return await _tsDbContext.Tests
            .Include(t => t.Company)
            .Where(t => t.CompanyId == companyId && !t.IsArchived)
            .OrderByDescending(t => t.CreatedOn)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Test>> GetPublicTestsAsync(CancellationToken ct)
    {
        return await _tsDbContext.Tests
            .Include(t => t.Company)
            .Where(t => t.IsPublic && t.IsActive && !t.IsArchived)
            .OrderByDescending(t => t.CreatedOn)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Test>> GetTestsByInviteCodeAsync(CancellationToken ct, string inviteCode)
    {
        return await _tsDbContext.Tests
            .Include(t => t.Company)
            .Where(t => t.InviteCode == inviteCode && t.IsActive && !t.IsArchived)
            .ToListAsync(ct);
    }

    public async Task<TestAnalyticsDto> GetTestAnalyticsAsync(CancellationToken ct, Guid testId)
    {
        var test = await _tsDbContext.Tests.FindAsync(testId);
        if (test == null) throw new ArgumentException("Test not found");

        var totalAttempts = await _tsDbContext.TestAttempts.CountAsync(ta => ta.TestId == testId, ct);
        var completedAttempts = await _tsDbContext.TestResults.CountAsync(tr => tr.TestId == testId, ct);
        var passedAttempts = await _tsDbContext.TestResults.CountAsync(tr => tr.TestId == testId && tr.Passed, ct);
        
        var averageScore = await _tsDbContext.TestResults
            .Where(tr => tr.TestId == testId)
            .AverageAsync(tr => (double?)tr.Score, ct) ?? 0;
            
        var passRate = completedAttempts > 0 ? (double)passedAttempts / completedAttempts : 0;
        
        var averageTime = await _tsDbContext.TestResults
            .Where(tr => tr.TestId == testId)
            .AverageAsync(tr => tr.TimeSpent.TotalSeconds, ct);

        return new TestAnalyticsDto(
            testId,
            test.Name,
            totalAttempts,
            completedAttempts,
            passedAttempts,
            averageScore,
            passRate,
            TimeSpan.FromSeconds(averageTime),
            new List<QuestionAnalyticsDto>() // Would be populated by GetQuestionAnalyticsAsync
        );
    }

    public async Task<IEnumerable<QuestionAnalyticsDto>> GetQuestionAnalyticsAsync(CancellationToken ct, Guid testId)
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
            
            var avgTime = await _tsDbContext.QuestionResults
                .Where(qr => qr.QuestionId == question.Id && qr.TimeSpent.HasValue)
                .AverageAsync(qr => qr.TimeSpent!.Value.TotalSeconds, ct);

            analytics.Add(new QuestionAnalyticsDto(
                question.Id,
                question.Text,
                question.Type.ToString(),
                totalResponses,
                correctResponses,
                successRate,
                TimeSpan.FromSeconds(avgTime),
                new List<AnswerAnalyticsDto>() // Would be populated with answer-specific analytics
            ));
        }

        return analytics;
    }

    public async Task<IEnumerable<Test>> GetTestsByIdsAsync(CancellationToken ct, IEnumerable<Guid> testIds)
    {
        return await _tsDbContext.Tests
            .Where(t => testIds.Contains(t.Id))
            .ToListAsync(ct);
    }

    public async Task BulkUpdateTestStatusAsync(CancellationToken ct, IEnumerable<Guid> testIds, bool isActive)
    {
        var tests = await GetTestsByIdsAsync(ct, testIds);
        foreach (var test in tests)
        {
            test.IsActive = isActive;
        }
        await _tsDbContext.SaveChangesAsync(ct);
    }
}