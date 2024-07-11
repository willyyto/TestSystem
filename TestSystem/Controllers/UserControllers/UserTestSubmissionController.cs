using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestSystem;
using TestSystem.Core.Dtos;
using TestSystem.Infra.Interfaces;
using TestSystem.Utils;

namespace QuizSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserTestSubmission : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly ITestService _TestService;

    public UserTestSubmission(ITestService TestService,
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

        try
        {
            var userId = UserUtils.GetUserId(User);
            var score = await _TestService.SubmitQuiz(ct, submission, userId);

            if (score == null) return NotFound("Test not found.");

            return Ok(new { Message = "Test submitted successfully", Score = score });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}