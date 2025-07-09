namespace TestSystem.Core.Dtos;

public record DashboardStatsDto(
    int TotalTests,
    int ActiveTests,
    int TotalUsers,
    int TotalAttempts,
    int RecentAttempts,
    double AverageScore,
    List<RecentActivityDto> RecentActivity
);