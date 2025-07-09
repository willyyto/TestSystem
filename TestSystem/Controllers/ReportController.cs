using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Extensions;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Controllers;

[Authorize(Roles = "Administrator,Manager")]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly ITestRepository _testRepository;
    private readonly IAnalyticsRepository _analyticsRepository;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(
        ITestRepository testRepository,
        IAnalyticsRepository analyticsRepository,
        ICancellationTokenAccessor cancellationTokenAccessor,
        ILogger<ReportsController> logger)
    {
        _testRepository = testRepository;
        _analyticsRepository = analyticsRepository;
        _cancellationTokenAccessor = cancellationTokenAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Generate comprehensive test report
    /// </summary>
    [HttpGet("test/{testId}")]
    [ProducesResponseType(typeof(ApiResponseDto<TestReportDto>), 200)]
    public async Task<IActionResult> GenerateTestReport(Guid testId, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            
            var test = await _testRepository.GetTestByIdAsync(ct, testId);
            if (test == null)
                return this.NotFoundResponse<string>("Test not found");

            var analytics = await _analyticsRepository.GetTestAnalyticsAsync(ct, testId, fromDate, toDate);
            var questionAnalytics = await _analyticsRepository.GetQuestionAnalyticsAsync(ct, testId);
            var testResults = await _testRepository.GetTestResultsAsync(ct, testId);
            
            var report = new TestReportDto
            {
                TestId = testId,
                TestName = test.Name,
                GeneratedAt = DateTime.UtcNow,
                DateRange = new DateRangeDto(fromDate, toDate),
                Analytics = analytics, // This is already a TestAnalyticsDto from the repository
                QuestionAnalytics = questionAnalytics.ToList(),
                Summary = new TestSummaryDto
                {
                    TotalAttempts = analytics.TotalAttempts,
                    CompletedAttempts = analytics.CompletedAttempts,
                    PassRate = analytics.PassRate,
                    AverageScore = analytics.AverageScore,
                    AverageTime = analytics.AverageCompletionTime
                }
            };

            return this.OkResponse(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating test report {TestId}", testId);
            return this.ExceptionResponse<TestReportDto>(ex);
        }
    }

    /// <summary>
    /// Generate company performance report
    /// </summary>
    [HttpGet("company/{companyId}")]
    [ProducesResponseType(typeof(ApiResponseDto<CompanyReportDto>), 200)]
    public async Task<IActionResult> GenerateCompanyReport(Guid companyId, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            
            var dashboardStats = await _analyticsRepository.GetDashboardStatsAsync(ct, companyId);
            var averageScore = await _analyticsRepository.GetCompanyAverageScoreAsync(ct, companyId, fromDate);
            
            var report = new CompanyReportDto
            {
                CompanyId = companyId,
                GeneratedAt = DateTime.UtcNow,
                DateRange = new DateRangeDto(fromDate, toDate),
                DashboardStats = dashboardStats, // This is already a DashboardStatsDto from the repository
                AverageScore = averageScore,
                Summary = new CompanySummaryDto
                {
                    TotalTests = dashboardStats.TotalTests,
                    TotalUsers = dashboardStats.TotalUsers,
                    TotalAttempts = dashboardStats.TotalAttempts,
                    OverallAverageScore = averageScore
                }
            };

            return this.OkResponse(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating company report {CompanyId}", companyId);
            return this.ExceptionResponse<CompanyReportDto>(ex);
        }
    }

    /// <summary>
    /// Export test results as CSV
    /// </summary>
    [HttpGet("export/test/{testId}/csv")]
    [ProducesResponseType(typeof(FileResult), 200)]
    public async Task<IActionResult> ExportTestResultsCsv(Guid testId, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var testResults = await _testRepository.GetTestResultsAsync(ct, testId);
            
            // Filter by date range if provided
            if (fromDate.HasValue)
                testResults = testResults.Where(tr => tr.CompletedDate >= fromDate.Value);
            if (toDate.HasValue)
                testResults = testResults.Where(tr => tr.CompletedDate <= toDate.Value);

            var csv = GenerateTestResultsCsv(testResults);
            var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
            
            return File(bytes, "text/csv", $"test-results-{testId}-{DateTime.UtcNow:yyyyMMdd}.csv");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting test results CSV {TestId}", testId);
            return this.ExceptionResponse<string>(ex);
        }
    }

    private string GenerateTestResultsCsv(IEnumerable<TestResult> testResults)
    {
        var csv = new System.Text.StringBuilder();
        csv.AppendLine("UserId,UserName,Score,Passed,CompletedDate,TimeSpent,QuestionsAnswered,QuestionsCorrect");
        
        foreach (var result in testResults)
        {
            csv.AppendLine($"{result.UserId},{result.UserId},{result.Score},{result.Passed},{result.CompletedDate:yyyy-MM-dd HH:mm:ss},{result.TimeSpent},{result.QuestionsAnswered},{result.QuestionsCorrect}");
        }
        
        return csv.ToString();
    }
}

// Alternative Report DTOs using classes instead of records for easier initialization
public class TestReportDto
{
    public Guid TestId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
    public DateRangeDto DateRange { get; set; } = new();
    public TestAnalyticsDto? Analytics { get; set; }
    public List<QuestionAnalyticsDto> QuestionAnalytics { get; set; } = new();
    public TestSummaryDto Summary { get; set; } = new();
}

public class CompanyReportDto
{
    public Guid CompanyId { get; set; }
    public DateTime GeneratedAt { get; set; }
    public DateRangeDto DateRange { get; set; } = new();
    public DashboardStatsDto? DashboardStats { get; set; }
    public double AverageScore { get; set; }
    public CompanySummaryDto Summary { get; set; } = new();
}

public class DateRangeDto
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    
    public DateRangeDto() { }
    public DateRangeDto(DateTime? fromDate, DateTime? toDate)
    {
        FromDate = fromDate;
        ToDate = toDate;
    }
}

public class TestSummaryDto
{
    public int TotalAttempts { get; set; }
    public int CompletedAttempts { get; set; }
    public double PassRate { get; set; }
    public double AverageScore { get; set; }
    public TimeSpan AverageTime { get; set; }
}

public class CompanySummaryDto
{
    public int TotalTests { get; set; }
    public int TotalUsers { get; set; }
    public int TotalAttempts { get; set; }
    public double OverallAverageScore { get; set; }
}