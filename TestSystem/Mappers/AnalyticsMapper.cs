using TestSystem.Core.Dtos;

namespace TestSystem.Mappers;

public static class AnalyticsMapper
{
    public static DashboardStatsDto MapToDashboardStatsDto(
        int totalTests,
        int activeTests,
        int totalUsers,
        int totalAttempts,
        int recentAttempts,
        double averageScore,
        List<RecentActivityDto> recentActivity)
    {
        return new DashboardStatsDto(
            totalTests,
            activeTests,
            totalUsers,
            totalAttempts,
            recentAttempts,
            averageScore,
            recentActivity
        );
    }

    public static RecentActivityDto MapToRecentActivityDto(
        string activityType,
        string description,
        DateTime timestamp,
        Guid? userId = null,
        string? userName = null)
    {
        return new RecentActivityDto(
            activityType,
            description,
            timestamp,
            userId,
            userName
        );
    }
}