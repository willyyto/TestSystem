using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Extensions;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly IAnalyticsRepository _analyticsRepository;
    private readonly ILogger<AnalyticsController> _logger;

    public AnalyticsController(
        IAnalyticsRepository analyticsRepository,
        ICancellationTokenAccessor cancellationTokenAccessor,
        ILogger<AnalyticsController> logger)
    {
        _analyticsRepository = analyticsRepository;
        _cancellationTokenAccessor = cancellationTokenAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Get dashboard statistics
    /// </summary>
    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(ApiResponseDto<DashboardStatsDto>), 200)]
    public async Task<IActionResult> GetDashboardStats([FromQuery] Guid? companyId = null)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var stats = await _analyticsRepository.GetDashboardStatsAsync(ct, companyId);
            
            return this.OkResponse(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dashboard stats");
            return this.ExceptionResponse<DashboardStatsDto>(ex);
        }
    }

    /// <summary>
    /// Get test analytics
    /// </summary>
    [HttpGet("tests/{testId}")]
    [ProducesResponseType(typeof(ApiResponseDto<TestAnalyticsDto>), 200)]
    public async Task<IActionResult> GetTestAnalytics(Guid testId, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var analytics = await _analyticsRepository.GetTestAnalyticsAsync(ct, testId, fromDate, toDate);
            
            return this.OkResponse(analytics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test analytics {TestId}", testId);
            return this.ExceptionResponse<TestAnalyticsDto>(ex);
        }
    }

    /// <summary>
    /// Get test attempts over time
    /// </summary>
    [HttpGet("tests/{testId}/attempts-over-time")]
    [ProducesResponseType(typeof(ApiResponseDto<Dictionary<DateTime, int>>), 200)]
    public async Task<IActionResult> GetTestAttemptsOverTime(Guid testId, [FromQuery] int days = 30)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var data = await _analyticsRepository.GetTestAttemptsOverTimeAsync(ct, testId, days);
            
            return this.OkResponse(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test attempts over time {TestId}", testId);
            return this.ExceptionResponse<Dictionary<DateTime, int>>(ex);
        }
    }

    /// <summary>
    /// Get score distribution for a test
    /// </summary>
    [HttpGet("tests/{testId}/score-distribution")]
    [ProducesResponseType(typeof(ApiResponseDto<Dictionary<string, double>>), 200)]
    public async Task<IActionResult> GetScoreDistribution(Guid testId)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var distribution = await _analyticsRepository.GetScoreDistributionAsync(ct, testId);
            
            return this.OkResponse(distribution);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving score distribution {TestId}", testId);
            return this.ExceptionResponse<Dictionary<string, double>>(ex);
        }
    }
}