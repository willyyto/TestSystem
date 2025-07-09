using TestSystem.Core.Dtos;

namespace TestSystem.Infra.Interfaces;

public interface IAnalyticsRepository
{
    Task<TestAnalyticsDto> GetTestAnalyticsAsync(CancellationToken ct, Guid testId, DateTime? fromDate = null, DateTime? toDate = null);
    Task<IEnumerable<QuestionAnalyticsDto>> GetQuestionAnalyticsAsync(CancellationToken ct, Guid testId);
    Task<DashboardStatsDto> GetDashboardStatsAsync(CancellationToken ct, Guid? companyId = null);
    Task<IEnumerable<RecentActivityDto>> GetRecentActivityAsync(CancellationToken ct, Guid? companyId = null, int limit = 10);
    
    // Advanced analytics
    Task<IDictionary<DateTime, int>> GetTestAttemptsOverTimeAsync(CancellationToken ct, Guid testId, int days = 30);
    Task<IDictionary<string, double>> GetScoreDistributionAsync(CancellationToken ct, Guid testId);
    Task<IEnumerable<(Guid UserId, string UserName, double AverageScore, int AttemptCount)>> GetTopPerformersAsync(CancellationToken ct, Guid testId, int limit = 10);
    Task<double> GetCompanyAverageScoreAsync(CancellationToken ct, Guid companyId, DateTime? fromDate = null);
}