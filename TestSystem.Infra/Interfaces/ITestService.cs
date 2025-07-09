using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface ITestService
{
    // Test taking flow
    Task<TestAttempt> StartTestAttemptAsync(CancellationToken ct, Guid testId, Guid userId, string? password = null);
    Task<TestResult?> SubmitTestAsync(CancellationToken ct, TestSubmissionDto submission, Guid userId);
    Task SaveProgressAsync(CancellationToken ct, Guid attemptId, Dictionary<Guid, string> answers);
    Task<TestAttempt> UpdateTestAttemptAsync(CancellationToken ct, TestAttempt attempt);
    Task AbandonTestAttemptAsync(CancellationToken ct, Guid attemptId);
    
    // Validation and scoring
    Task<ValidationResultDto> ValidateTestSubmissionAsync(CancellationToken ct, TestSubmissionDto submission);
    Task<double> CalculateTestScoreAsync(CancellationToken ct, Test test, TestSubmissionDto submission);
    Task<bool> ValidateAnswerAsync(CancellationToken ct, Question question, string answer);
    
    // Test management
    Task<Test> CreateTestAsync(CancellationToken ct, CreateTestDto testDto);
    Task<Test> UpdateTestAsync(CancellationToken ct, Guid testId, CreateTestDto testDto);
    Task<bool> DuplicateTestAsync(CancellationToken ct, Guid testId, string newName, Guid? targetCompanyId = null);
    
    // Analytics and reporting
    Task<TestAnalyticsDto> GetTestAnalyticsAsync(CancellationToken ct, Guid testId);
    Task<byte[]> ExportTestResultsAsync(CancellationToken ct, Guid testId, string format = "csv");
    Task<string> GenerateCertificateAsync(CancellationToken ct, Guid testResultId);
    
    // Import/Export
    Task<Test> ImportTestAsync(CancellationToken ct, TestImportDto importDto);
    Task<string> ExportTestAsync(CancellationToken ct, TestExportDto exportDto);
}