using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Extensions;
using TestSystem.Infra.Interfaces;
using TestSystem.Mappers;
using TestSystem.Utils;

namespace TestSystem.Controllers.UserControllers;

[Authorize]
[ApiController]
[Route("api/user/[controller]")]
public class UserTestResultController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly ITestRepository _testRepository;
    private readonly ITestService _testService;
    private readonly ILogger<UserTestResultController> _logger;

    public UserTestResultController(
        ITestRepository testRepository,
        ITestService testService,
        ICancellationTokenAccessor cancellationTokenAccessor,
        ILogger<UserTestResultController> logger)
    {
        _testRepository = testRepository;
        _testService = testService;
        _cancellationTokenAccessor = cancellationTokenAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Get user's test results
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<List<TestResultDto>>), 200)]
    public async Task<IActionResult> GetTestResults()
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var userId = UserUtils.GetUserId(User);
            
            var testResults = await _testRepository.GetTestResultsByUserIdAsync(ct, userId);
            var resultDtos = testResults.Select(tr => tr.MapToTestResultDto()).ToList();
            
            return this.OkResponse(resultDtos);
        }
        catch (UnauthorizedAccessException)
        {
            return this.UnauthorizedResponse<string>("Access denied");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test results");
            return this.ExceptionResponse<List<TestResultDto>>(ex);
        }
    }

    /// <summary>
    /// Get specific test result
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<TestResultDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> GetTestResult(Guid id)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var userId = UserUtils.GetUserId(User);
            
            var testResult = await _testRepository.GetTestResultByIdAndUserIdAsync(ct, id, userId);
            if (testResult == null)
                return this.NotFoundResponse<string>("Test result not found");
            
            return this.OkResponse(testResult.MapToTestResultDto());
        }
        catch (UnauthorizedAccessException)
        {
            return this.UnauthorizedResponse<string>("Access denied");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test result {ResultId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Generate certificate for passed test
    /// </summary>
    [HttpPost("{id}/certificate")]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 400)]
    public async Task<IActionResult> GenerateCertificate(Guid id)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var userId = UserUtils.GetUserId(User);
            
            var testResult = await _testRepository.GetTestResultByIdAndUserIdAsync(ct, id, userId);
            if (testResult == null)
                return this.NotFoundResponse<string>("Test result not found");
            
            if (!testResult.Passed)
                return this.BadRequestResponse<string>("Cannot generate certificate for failed test");
            
            var certificateUrl = await _testService.GenerateCertificateAsync(ct, id);
            return this.OkResponse(certificateUrl, "Certificate generated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating certificate for result {ResultId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }
}