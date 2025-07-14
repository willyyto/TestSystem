using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Extensions;
using TestSystem.Filters;
using TestSystem.Infra.Interfaces;
using TestSystem.Mappers;

namespace TestSystem.Controllers;
[Authorize(Roles = "admin, Manager")]
[ApiController]
[Route("api/admin/[controller]")]
public class AdminTestController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly ITestRepository _testRepository;
    private readonly ITestService _testService;
    private readonly ILogger<AdminTestController> _logger;

    public AdminTestController(
        ITestRepository testRepository,
        ITestService testService,
        ICancellationTokenAccessor cancellationTokenAccessor,
        ILogger<AdminTestController> logger)
    {
        _testRepository = testRepository;
        _testService = testService;
        _cancellationTokenAccessor = cancellationTokenAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Get all tests with optional filtering
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<PagedResultDto<TestDto>>), 200)]
    public async Task<IActionResult> GetTests([FromQuery] TestSearchDto searchDto)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var pagedResult = await _testRepository.SearchTestsAsync(ct, searchDto);
            var testDtos = pagedResult.MapToPagedResult(test => test.MapToTestDto());
            
            return this.OkResponse(testDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tests");
            return this.ExceptionResponse<PagedResultDto<TestDto>>(ex);
        }
    }

    /// <summary>
    /// Get test by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<TestDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> GetTest(Guid id)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var test = await _testRepository.GetTestByIdAsync(ct, id);
            
            if (test == null)
                return this.NotFoundResponse<string>("Test not found");

            return this.OkResponse(test.MapToTestDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test {TestId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Create a new test
    /// </summary>
    [HttpPost]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<TestDto>), 201)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 400)]
    public async Task<IActionResult> CreateTest([FromBody] CreateTestDto testDto)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var test = await _testService.CreateTestAsync(ct, testDto);
            
            var testDtoResult = test.MapToTestDto();
            return this.CreatedResponse(nameof(GetTest), new { id = test.Id }, testDtoResult, "Test created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test");
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Update an existing test
    /// </summary>
    [HttpPut("{id}")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<TestDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> UpdateTest(Guid id, [FromBody] CreateTestDto testDto)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var updatedTest = await _testService.UpdateTestAsync(ct, id, testDto);
            
            return this.OkResponse(updatedTest.MapToTestDto(), "Test updated successfully");
        }
        catch (ArgumentException)
        {
            return this.NotFoundResponse<string>("Test not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test {TestId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Delete a test (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> DeleteTest(Guid id)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var deletedTest = await _testRepository.DeleteTestByIdAsync(ct, id);
            
            if (deletedTest == null)
                return this.NotFoundResponse<string>("Test not found");

            _logger.LogInformation("Test deleted: {TestId}", id);
            return this.OkResponse("Test deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test {TestId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Duplicate a test
    /// </summary>
    [HttpPost("{id}/duplicate")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> DuplicateTest(Guid id, [FromBody] DuplicateTestRequest request)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var success = await _testService.DuplicateTestAsync(ct, id, request.NewName, request.TargetCompanyId);
            
            if (!success)
                return this.NotFoundResponse<string>("Test not found");

            return this.OkResponse("Test duplicated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error duplicating test {TestId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Get test analytics
    /// </summary>
    [HttpGet("{id}/analytics")]
    [ProducesResponseType(typeof(ApiResponseDto<TestAnalyticsDto>), 200)]
    public async Task<IActionResult> GetTestAnalytics(Guid id)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var analytics = await _testService.GetTestAnalyticsAsync(ct, id);
            
            return this.OkResponse(analytics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test analytics {TestId}", id);
            return this.ExceptionResponse<TestAnalyticsDto>(ex);
        }
    }

    /// <summary>
    /// Export test results
    /// </summary>
    [HttpGet("{id}/export")]
    [ProducesResponseType(typeof(FileResult), 200)]
    public async Task<IActionResult> ExportTestResults(Guid id, [FromQuery] string format = "csv")
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var data = await _testService.ExportTestResultsAsync(ct, id, format);
            
            var contentType = format.ToLower() switch
            {
                "csv" => "text/csv",
                "excel" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "pdf" => "application/pdf",
                _ => "application/octet-stream"
            };

            var fileName = $"test-results-{id}.{format}";
            return File(data, contentType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting test results {TestId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }
}
