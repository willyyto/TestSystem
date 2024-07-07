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
        return await _tsDbContext.Tests.Include(q => q.Questions).ThenInclude(q => q.Answers).ToListAsync(ct);
    }

    public async Task<Test?> GetTestByIdAsync(CancellationToken ct, Guid id)
    {
        return await _tsDbContext.Tests.Include(q => q.Questions).ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<IEnumerable<TestResult>> GetTestResultsAsync(CancellationToken ct)
    {
        return await _tsDbContext.TestResults
            .Include(r => r.Test)
            .Include(r => r.QuestionResults).ToListAsync(ct);
    }

    public async Task<TestResult?> GetTestResultByIdAsync(CancellationToken ct, Guid id)
    {
        return await _tsDbContext.TestResults
            .Include(r => r.Test)
            .Include(r => r.QuestionResults).FirstOrDefaultAsync(t => t.Id == id, ct);
    }
}