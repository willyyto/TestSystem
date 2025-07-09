using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Extensions;
using TestSystem.Filters;
using TestSystem.Infra.Interfaces;
using TestSystem.Mappers;

namespace TestSystem.Controllers;

[Authorize(Roles = "Administrator,Manager")]
[ApiController]
[Route("api/admin/[controller]")]
public class AdminQuestionController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly IQuestionRepository _questionRepository;
    private readonly ILogger<AdminQuestionController> _logger;

    public AdminQuestionController(
        IQuestionRepository questionRepository,
        ICancellationTokenAccessor cancellationTokenAccessor,
        ILogger<AdminQuestionController> logger)
    {
        _questionRepository = questionRepository;
        _cancellationTokenAccessor = cancellationTokenAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Get questions for a test
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<List<QuestionDto>>), 200)]
    public async Task<IActionResult> GetQuestions([FromQuery] Guid? testId = null)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var questions = await _questionRepository.GetQuestionsAsync(ct, testId);
            var questionDtos = questions.Select(q => q.MapToQuestionDto()).ToList();
            
            return this.OkResponse(questionDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving questions");
            return this.ExceptionResponse<List<QuestionDto>>(ex);
        }
    }

    /// <summary>
    /// Get question by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<QuestionDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> GetQuestion(Guid id)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var question = await _questionRepository.GetQuestionByIdAsync(ct, id);
            
            if (question == null)
                return this.NotFoundResponse<string>("Question not found");

            return this.OkResponse(question.MapToQuestionDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving question {QuestionId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Create a new question
    /// </summary>
    [HttpPost]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<QuestionDto>), 201)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 400)]
    public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionDto questionDto)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var question = questionDto.MapToQuestion();
            var createdQuestion = await _questionRepository.CreateQuestionAsync(ct, question);
            
            var questionDtoResult = createdQuestion.MapToQuestionDto();
            return this.CreatedResponse(nameof(GetQuestion), new { id = createdQuestion.Id }, 
                questionDtoResult, "Question created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating question");
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Update an existing question
    /// </summary>
    [HttpPut("{id}")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<QuestionDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> UpdateQuestion(Guid id, [FromBody] CreateQuestionDto questionDto)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var existingQuestion = await _questionRepository.GetQuestionByIdAsync(ct, id);
            
            if (existingQuestion == null)
                return this.NotFoundResponse<string>("Question not found");

            // Update question properties
            existingQuestion.Text = questionDto.Text;
            existingQuestion.Type = Enum.Parse<QuestionType>(questionDto.Type);
            existingQuestion.Weight = questionDto.Weight;
            existingQuestion.TimeLimit = questionDto.TimeLimit;
            existingQuestion.IsRequired = questionDto.IsRequired;
            existingQuestion.ImageUrl = questionDto.ImageUrl;
            existingQuestion.VideoUrl = questionDto.VideoUrl;
            existingQuestion.AudioUrl = questionDto.AudioUrl;
            existingQuestion.Explanation = questionDto.Explanation;
            existingQuestion.Hint = questionDto.Hint;
            existingQuestion.DisplayOrder = questionDto.DisplayOrder;
            existingQuestion.UpdatedOn = DateTime.UtcNow;

            var updatedQuestion = await _questionRepository.UpdateQuestionAsync(ct, existingQuestion);
            return this.OkResponse(updatedQuestion.MapToQuestionDto(), "Question updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating question {QuestionId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Delete a question
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> DeleteQuestion(Guid id)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var success = await _questionRepository.DeleteQuestionAsync(ct, id);
            
            if (!success)
                return this.NotFoundResponse<string>("Question not found");

            _logger.LogInformation("Question deleted: {QuestionId}", id);
            return this.OkResponse("Question deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting question {QuestionId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Duplicate a question to another test
    /// </summary>
    [HttpPost("{id}/duplicate")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<QuestionDto>), 200)]
    public async Task<IActionResult> DuplicateQuestion(Guid id, [FromBody] DuplicateQuestionRequest request)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var duplicatedQuestion = await _questionRepository.DuplicateQuestionAsync(ct, id, request.TargetTestId);
            
            return this.OkResponse(duplicatedQuestion.MapToQuestionDto(), "Question duplicated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error duplicating question {QuestionId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Bulk create questions
    /// </summary>
    [HttpPost("bulk")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<List<QuestionDto>>), 200)]
    public async Task<IActionResult> BulkCreateQuestions([FromBody] List<CreateQuestionDto> questionDtos)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var questions = questionDtos.Select(dto => dto.MapToQuestion()).ToList();
            var createdQuestions = await _questionRepository.BulkCreateQuestionsAsync(ct, questions);
            
            var questionDtoResults = createdQuestions.Select(q => q.MapToQuestionDto()).ToList();
            return this.OkResponse(questionDtoResults, "Questions created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk creating questions");
            return this.ExceptionResponse<string>(ex);
        }
    }
}