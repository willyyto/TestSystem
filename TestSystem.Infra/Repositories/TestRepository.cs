using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            .ThenInclude(q => q.MatchPairs)
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .Where(t => (t.IsActive == true && t.IsArchived == false) )
            .OrderByDescending(t => t.Name)
            .ToListAsync(ct);
    }

    public async Task<Test?> GetTestByIdAsync(CancellationToken ct, Guid id)
    {
        return await _tsDbContext.Tests
            .Include(t => t.Company)
            .Include(t => t.Questions)
            .ThenInclude(q => q.MatchPairs)
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .Where(t => (t.IsActive == true && t.IsArchived == false) )
            .OrderByDescending(t => t.Name)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }
    
    public async Task<Test?> DeleteTestByIdAsync(CancellationToken ct, Guid id)
    {
        var test = await _tsDbContext.Tests
            .Include(t => t.Company)
            .Include(t => t.Questions)
            .ThenInclude(q => q.MatchPairs)
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .Where(t => (t.IsActive == true && t.IsArchived == false) )
            .OrderByDescending(t => t.Name)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
        
        if (test == null) return null;
        
        _tsDbContext.TestResults.RemoveRange(test.TestResults);
        _tsDbContext.Tests.Remove(test);
        await _tsDbContext.SaveChangesAsync(ct);
        
        return test;
    }

    public async Task<IEnumerable<TestResult>> GetTestResultsAsync(CancellationToken ct)
    {
        return await _tsDbContext.TestResults
            .Include(r => r.Test)
            .ThenInclude(c => c.Company)
            .Include(r => r.QuestionResults)
            .ThenInclude(q => q.Question)
            .ThenInclude(a => a.Answers).ToListAsync(ct);
    }

    public async Task<TestResult?> GetTestResultByIdAsync(CancellationToken ct, Guid id)
    {
        return await _tsDbContext.TestResults
            .Include(r => r.Test)
            .ThenInclude(c => c.Company)
            .Include(r => r.QuestionResults)
            .ThenInclude(q => q.Question)
            .ThenInclude(a => a.Answers).FirstOrDefaultAsync(t => t.Id == id, ct);
    }
    
    public async Task<IEnumerable<TestResult>> GetTestResultsByUserIdAsync(CancellationToken ct, Guid userId)
    {
        return await _tsDbContext.TestResults
            .Include(r => r.Test)
            .ThenInclude(c => c.Company)
            .Include(r => r.QuestionResults)
            .ThenInclude(q => q.Question)
            .ThenInclude(a => a.Answers)
            .Where(tr => tr.UserId == userId)
            .ToListAsync(ct);
    }

    public async Task<TestResult?> GetTestResultByIdAndUserIdAsync(CancellationToken ct, Guid id, Guid userId)
    {
        return await _tsDbContext.TestResults
            .Include(r => r.Test)
            .ThenInclude(c => c.Company)
            .Include(r => r.QuestionResults)
            .ThenInclude(q => q.Question)
            .ThenInclude(a => a.Answers)
            .FirstOrDefaultAsync(tr => tr.Id == id && tr.UserId == userId, ct);
    }
}
