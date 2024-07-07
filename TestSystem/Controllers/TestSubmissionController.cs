using Microsoft.AspNetCore.Mvc;
using TestSystem;
using TestSystem.Core.Dtos;
using TestSystem.Infra.Interfaces;

namespace QuizSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestSubmission : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly ITestService _TestService;

    public TestSubmission(ITestService TestService,
        ICancellationTokenAccessor cancellationTokenAccessor)
    {
        _TestService = TestService;
        _cancellationTokenAccessor = cancellationTokenAccessor;
    }

    [HttpPost("submit")]
    public async Task<ActionResult> SubmitTest(TestSubmissionDto submission)
    {
        var ct = _cancellationTokenAccessor.Token;
        if (submission == null || submission.TestId == Guid.Empty || submission.Answers == null)
            return BadRequest("Invalid submission data.");

        var score = _TestService.SubmitQuiz(ct, submission);

        if (score == null) return NotFound("Test not found.");


        return Ok(new {Message = "Test submitted successfully", Score = score});
    }
}