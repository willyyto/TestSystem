using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface ITestRepository
{
    Task<IEnumerable<Test>> GetTestsAsync(CancellationToken ct);
    Task<Test?> GetTestByIdAsync(CancellationToken ct, Guid id);
    Task<IEnumerable<TestResult>> GetTestResultsAsync(CancellationToken ct);
    Task<TestResult?> GetTestResultByIdAsync(CancellationToken ct, Guid id);
}