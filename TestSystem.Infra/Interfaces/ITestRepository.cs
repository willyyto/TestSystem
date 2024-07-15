using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface ITestRepository
{
    Task<IEnumerable<Test>> GetTestsAsync(CancellationToken ct);
    Task<Test?> GetTestByIdAsync(CancellationToken ct, Guid id);
    Task<Test?> DeleteTestByIdAsync(CancellationToken ct, Guid id);
    Task<IEnumerable<TestResult>> GetTestResultsAsync(CancellationToken ct);
    Task<TestResult?> GetTestResultByIdAsync(CancellationToken ct, Guid id);
    Task<IEnumerable<TestResult>> GetTestResultsByUserIdAsync(CancellationToken ct, Guid userId);
    Task<TestResult?> GetTestResultByIdAndUserIdAsync(CancellationToken ct, Guid id, Guid userId);
}