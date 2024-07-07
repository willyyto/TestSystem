using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Entities;
using TestSystem.Infra.Interfaces;
using TestSystem.Mappers;

namespace TestSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly ITestRepository _TestRepository;

    public TestController(ITestRepository TestRepository,
        ICancellationTokenAccessor cancellationTokenAccessor)
    {
        _TestRepository = TestRepository;
        _cancellationTokenAccessor = cancellationTokenAccessor;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Test>>> GetTests()
    {
        var ct = _cancellationTokenAccessor.Token;
        var tests = await _TestRepository.GetTestsAsync(ct);
        return Ok(tests.Select(i => i.MapToTestDto()).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Test>> GetTest(Guid id)
    {
        var ct = _cancellationTokenAccessor.Token;
        var test = await _TestRepository.GetTestByIdAsync(ct, id);
        if (test == null) return NotFound();
        return Ok(test.MapToTestDto());
    }
}