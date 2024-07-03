using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Entities;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly IQuestionRepository _questionRepository;

    public QuestionsController(IQuestionRepository questionRepository,
        ICancellationTokenAccessor cancellationTokenAccessor)
    {
        _questionRepository = questionRepository;
        _cancellationTokenAccessor = cancellationTokenAccessor;
    }

    [HttpGet]
    [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
    {
        var ct = _cancellationTokenAccessor.Token;
        var questions = await _questionRepository.GetQuestionsAsync(ct);
        return Ok(questions);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<Question>> AddQuestion([FromBody] Question question)
    {
        var ct = _cancellationTokenAccessor.Token;
        var createdQuestion = await _questionRepository.AddQuestionAsync(ct, question);
        return CreatedAtAction(nameof(GetQuestions), new {id = createdQuestion.Id}, createdQuestion);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult<Question>> GetQuestionById(Guid id)
    {
        var ct = _cancellationTokenAccessor.Token;
        var question = await _questionRepository.GetQuestionByIdAsync(ct, id);
        if (question == null)
            return NotFound();

        return Ok(question);
    }
}