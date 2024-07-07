using Microsoft.AspNetCore.Mvc;
using TestSystem;
using TestSystem.Core.Entities;
using TestSystem.Infra.Interfaces;
using TestSystem.Mappers;

namespace TestResultSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestResultController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly ITestRepository _TestResultRepository;

    public TestResultController(ITestRepository TestResultRepository,
        ICancellationTokenAccessor cancellationTokenAccessor)
    {
        _TestResultRepository = TestResultRepository;
        _cancellationTokenAccessor = cancellationTokenAccessor;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestResult>>> GetTestResults()
    {
        var ct = _cancellationTokenAccessor.Token;
        var testResults = await _TestResultRepository.GetTestResultsAsync(ct);
        return Ok(testResults.Select(tr => tr.MapToTestResultDto()).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TestResult>> GetTestResult(Guid id)
    {
        var ct = _cancellationTokenAccessor.Token;
        var testResult = await _TestResultRepository.GetTestResultByIdAsync(ct, id);
        if (testResult == null) return NotFound();
        return Ok(testResult.MapToTestResultDto());
    }
}