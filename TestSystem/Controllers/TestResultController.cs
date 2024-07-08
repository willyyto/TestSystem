using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestSystem;
using TestSystem.Core.Entities;
using TestSystem.Infra.Interfaces;
using TestSystem.Mappers;
using TestSystem.Utils;

namespace TestResultSystem.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TestResultController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly ITestRepository _TestRepository;

    public TestResultController(ITestRepository TestRepository,
        ICancellationTokenAccessor cancellationTokenAccessor)
    {
        _TestRepository = TestRepository;
        _cancellationTokenAccessor = cancellationTokenAccessor;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestResult>>> GetTestResults()
    {
        var ct = _cancellationTokenAccessor.Token;
        try
        {
            var userId = UserUtils.GetUserId(User);
            var testResults = await _TestRepository.GetTestResultsByUserIdAsync(ct, userId);
            return Ok(testResults.Select(tr => tr.MapToTestResultDto()).ToList());
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TestResult>> GetTestResult(Guid id)
    {
        var ct = _cancellationTokenAccessor.Token;
        try
        {
            var userId = UserUtils.GetUserId(User);
            var testResult = await _TestRepository.GetTestResultByIdAndUserIdAsync(ct, id, userId);
            if (testResult == null) return NotFound();
            return Ok(testResult.MapToTestResultDto());
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}