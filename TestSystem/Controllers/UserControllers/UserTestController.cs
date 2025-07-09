using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Extensions;
using TestSystem.Filters;
using TestSystem.Infra.Interfaces;
using TestSystem.Mappers;
using TestSystem.Utils;

namespace TestSystem.Controllers.UserControllers;

[Authorize]
[ApiController]
[Route("api/user/[controller]")]
public class UserTestController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly ITestRepository _testRepository;
    private readonly ITestService _testService;
    private readonly ILogger<UserTestController> _logger;

    public UserTestController(
        ITestRepository testRepository,
        ITestService testService,
        ICancellationTokenAccessor cancellationTokenAccessor,
        ILogger<UserTestController> logger)
    {
        _testRepository = testRepository;
        _testService = testService;
        _cancellationTokenAccessor = cancellationTokenAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Get available tests for the user
    /// </summary>
    [HttpGet("available")]
    [ProducesResponseType(typeof(ApiResponseDto<List<TestDto>>), 200)]
    public async Task<IActionResult> GetAvailableTests()
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var userId = UserUtils.GetUserId(User);
            
            var publicTests = await _testRepository.GetPublicTestsAsync(ct);
            var tests = publicTests.Where(t => 
                (t.AvailableFrom == null || t.AvailableFrom <= DateTime.UtcNow) &&
                (t.AvailableUntil == null || t.AvailableUntil >= DateTime.UtcNow))
                .Select(t => t.MapToTestDto())
                .ToList();

            return this.OkResponse(tests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available tests");
            return this.ExceptionResponse<List<TestDto>>(ex);
        }
    }

    /// <summary>
    /// Get test by ID for taking
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<TestDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> GetTest(Guid id, [FromQuery] string? password = null)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var test = await _testRepository.GetTestForTakingAsync(ct, id, password);
            
            if (test == null)
                return this.NotFoundResponse<string>("Test not found or not accessible");

            var testDto = test.MapToTestDto();
            return this.OkResponse(testDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test {TestId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Start a test attempt
    /// </summary>
    [HttpPost("{id}/start")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<TestAttemptDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 400)]
    public async Task<IActionResult> StartTest(Guid id, [FromBody] StartTestRequest request)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var userId = UserUtils.GetUserId(User);
            
            var attempt = await _testService.StartTestAttemptAsync(ct, id, userId, request.Password);
            
            return this.OkResponse(attempt.MapToTestAttemptDto(), "Test started successfully");
        }
        catch (UnauthorizedAccessException ex)
        {
            return this.UnauthorizedResponse<string>(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return this.BadRequestResponse<string>(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting test {TestId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Submit test answers
    /// </summary>
    [HttpPost("{id}/submit")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<TestResultDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 400)]
    public async Task<IActionResult> SubmitTest(Guid id, [FromBody] TestSubmissionDto submission)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var userId = UserUtils.GetUserId(User);
            
            if (submission.TestId != id)
                return this.BadRequestResponse<string>("Test ID mismatch");

            var result = await _testService.SubmitTestAsync(ct, submission, userId);
            
            if (result == null)
                return this.BadRequestResponse<string>("Failed to submit test");

            return this.OkResponse(result.MapToTestResultDto(), "Test submitted successfully");
        }
        catch (ArgumentException ex)
        {
            return this.BadRequestResponse<string>(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting test {TestId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Save test progress
    /// </summary>
    [HttpPost("attempts/{attemptId}/save-progress")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    public async Task<IActionResult> SaveProgress(Guid attemptId, [FromBody] Dictionary<Guid, string> answers)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            await _testService.SaveProgressAsync(ct, attemptId, answers);
            
            return this.OkResponse("Progress saved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving progress for attempt {AttemptId}", attemptId);
            return this.ExceptionResponse<string>(ex);
        }
    }
    
    /// <summary>
    /// Abandon test attempt
    /// </summary>
    [HttpPost("attempts/{attemptId}/abandon")]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    public async Task<IActionResult> AbandonTest(Guid attemptId)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            await _testService.AbandonTestAttemptAsync(ct, attemptId);
            
            return this.OkResponse("Test abandoned");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error abandoning test attempt {AttemptId}", attemptId);
            return this.ExceptionResponse<string>(ex);
        }
    }
}