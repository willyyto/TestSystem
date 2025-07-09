using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface ITestRepository
{
    // Basic CRUD operations
    Task<IEnumerable<Test>> GetTestsAsync(CancellationToken ct);
    Task<Test?> GetTestByIdAsync(CancellationToken ct, Guid id);
    Task<Test> CreateTestAsync(CancellationToken ct, Test test);
    Task<Test> UpdateTestAsync(CancellationToken ct, Test test);
    Task<Test?> DeleteTestByIdAsync(CancellationToken ct, Guid id);
    
    // Test access and validation
    Task<Test?> GetTestForTakingAsync(CancellationToken ct, Guid id, string? password = null);
    Task<bool> CanUserTakeTestAsync(CancellationToken ct, Guid testId, Guid userId);
    Task<int> GetUserAttemptCountAsync(CancellationToken ct, Guid testId, Guid userId);
    
    // Test results and attempts
    Task<IEnumerable<TestResult>> GetTestResultsAsync(CancellationToken ct, Guid? testId = null, Guid? userId = null);
    Task<TestResult?> GetTestResultByIdAsync(CancellationToken ct, Guid id);
    Task<IEnumerable<TestResult>> GetTestResultsByUserIdAsync(CancellationToken ct, Guid userId);
    Task<TestResult?> GetTestResultByIdAndUserIdAsync(CancellationToken ct, Guid id, Guid userId);
    Task<TestResult> CreateTestResultAsync(CancellationToken ct, TestResult testResult);
    Task<TestResult> UpdateTestResultAsync(CancellationToken ct, TestResult testResult);
    
    // Test attempts
    Task<TestAttempt> CreateTestAttemptAsync(CancellationToken ct, TestAttempt attempt);
    Task<TestAttempt> UpdateTestAttemptAsync(CancellationToken ct, TestAttempt attempt);
    Task<TestAttempt?> GetActiveTestAttemptAsync(CancellationToken ct, Guid testId, Guid userId);
    Task<IEnumerable<TestAttempt>> GetTestAttemptsAsync(CancellationToken ct, Guid testId, Guid? userId = null);
    
    // Search and filtering
    Task<PagedResultDto<Test>> SearchTestsAsync(CancellationToken ct, TestSearchDto searchDto);
    Task<IEnumerable<Test>> GetTestsByCompanyAsync(CancellationToken ct, Guid companyId);
    Task<IEnumerable<Test>> GetPublicTestsAsync(CancellationToken ct);
    Task<IEnumerable<Test>> GetTestsByInviteCodeAsync(CancellationToken ct, string inviteCode);
    
    // Analytics
    Task<TestAnalyticsDto> GetTestAnalyticsAsync(CancellationToken ct, Guid testId);
    Task<IEnumerable<QuestionAnalyticsDto>> GetQuestionAnalyticsAsync(CancellationToken ct, Guid testId);
    
    // Bulk operations
    Task<IEnumerable<Test>> GetTestsByIdsAsync(CancellationToken ct, IEnumerable<Guid> testIds);
    Task BulkUpdateTestStatusAsync(CancellationToken ct, IEnumerable<Guid> testIds, bool isActive);
}