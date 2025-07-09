using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Infra.DataServices;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Infra.Repositories;

// Enhanced TestService Implementation
[InstanceScopedService]
public class TestService : ITestService
{
    private readonly ITestSystemDbContextAsync _tsDbContext;
    private readonly ITestRepository _testRepository;
    private readonly ILogger<TestService> _logger;

    public TestService(
        ITestSystemDbContextAsync tsDbContext,
        ITestRepository testRepository,
        ILogger<TestService> logger)
    {
        _tsDbContext = tsDbContext;
        _testRepository = testRepository;
        _logger = logger;
    }

    public async Task<TestAttempt> StartTestAttemptAsync(CancellationToken ct, Guid testId, Guid userId, string? password = null)
    {
        // Validate test access
        var test = await _testRepository.GetTestForTakingAsync(ct, testId, password);
        if (test == null)
            throw new UnauthorizedAccessException("Test not accessible");

        if (!await _testRepository.CanUserTakeTestAsync(ct, testId, userId))
            throw new InvalidOperationException("Maximum attempts exceeded");

        // Check for existing active attempt
        var existingAttempt = await _testRepository.GetActiveTestAttemptAsync(ct, testId, userId);
        if (existingAttempt != null)
            return existingAttempt;

        // Create new attempt
        var attemptNumber = await _testRepository.GetUserAttemptCountAsync(ct, testId, userId) + 1;
        
        var attempt = new TestAttempt
        {
            Id = Guid.NewGuid(),
            TestId = testId,
            UserId = userId,
            StartedAt = DateTime.UtcNow,
            AttemptNumber = attemptNumber,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        return await _testRepository.CreateTestAttemptAsync(ct, attempt);
    }

    public async Task<TestResult?> SubmitTestAsync(CancellationToken ct, TestSubmissionDto submission, Guid userId)
    {
        var test = await _tsDbContext.Tests
            .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
            .Include(t => t.Questions)
                .ThenInclude(q => q.MatchPairs)
            .Include(t => t.Questions)
                .ThenInclude(q => q.OrderingItems)
            .FirstOrDefaultAsync(t => t.Id == submission.TestId, ct);

        if (test == null) return null;

        // Validate submission
        var validationResult = await ValidateTestSubmissionAsync(ct, submission);
        if (!validationResult.IsValid)
            throw new ArgumentException($"Invalid submission: {string.Join(", ", validationResult.Errors.Select(e => e.Message))}");

        // Get the active attempt
        var attempt = await _testRepository.GetActiveTestAttemptAsync(ct, submission.TestId, userId);
        if (attempt == null)
            throw new InvalidOperationException("No active test attempt found");

        // Calculate scores and process answers
        var questionResults = new List<QuestionResult>();
        double totalPoints = 0;
        double earnedPoints = 0;
        int questionsAnswered = 0;
        int questionsCorrect = 0;
        int questionsSkipped = 0;

        foreach (var question in test.Questions)
        {
            var maxPoints = question.Weight;
            totalPoints += maxPoints;
            
            var questionResult = new QuestionResult
            {
                Id = Guid.NewGuid(),
                QuestionId = question.Id,
                MaxPoints = maxPoints,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };

            // Process answer based on question type
            var (isCorrect, points, answer, requiresManualGrading) = await ProcessQuestionAnswerAsync(ct, question, submission);
            
            questionResult.IsCorrect = isCorrect;
            questionResult.PointsEarned = points;
            questionResult.Answer = answer;
            questionResult.RequiresManualGrading = requiresManualGrading;
            
            // Set time spent if available
            if (submission.QuestionTimes.TryGetValue(question.Id, out var timeSpent))
                questionResult.TimeSpent = timeSpent;

            if (!string.IsNullOrEmpty(answer))
            {
                questionsAnswered++;
                if (isCorrect) questionsCorrect++;
                earnedPoints += points;
            }
            else
            {
                questionsSkipped++;
                questionResult.IsSkipped = true;
            }

            questionResults.Add(questionResult);
        }

        // Calculate final score
        var scorePercentage = totalPoints > 0 ? (int)Math.Round((earnedPoints / totalPoints) * 100) : 0;
        var passed = scorePercentage >= test.PassMark;
        
        // Calculate total time spent
        var totalTimeSpent = attempt.CompletedAt.HasValue 
            ? attempt.CompletedAt.Value - attempt.StartedAt 
            : DateTime.UtcNow - attempt.StartedAt;

        // Create test result
        var testResult = new TestResult
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TestId = test.Id,
            TestAttemptId = attempt.Id,
            CompletedDate = DateTime.UtcNow,
            Score = scorePercentage,
            RawScore = earnedPoints,
            MaxPossibleScore = totalPoints,
            Passed = passed,
            TimeSpent = totalTimeSpent,
            QuestionsAnswered = questionsAnswered,
            QuestionsCorrect = questionsCorrect,
            QuestionsSkipped = questionsSkipped,
            Grade = CalculateGrade(test.GradingScheme, scorePercentage),
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            QuestionResults = questionResults
        };

        // Mark attempt as completed
        attempt.IsCompleted = true;
        attempt.CompletedAt = DateTime.UtcNow;
        attempt.TimeSpent = totalTimeSpent;
        await _testRepository.UpdateTestAttemptAsync(ct, attempt);

        // Save test result
        await _testRepository.CreateTestResultAsync(ct, testResult);

        _logger.LogInformation("Test submitted successfully. User: {UserId}, Test: {TestId}, Score: {Score}%", 
            userId, test.Id, scorePercentage);

        return testResult;
    }

    private async Task<(bool IsCorrect, double Points, string Answer, bool RequiresManualGrading)> ProcessQuestionAnswerAsync(
        CancellationToken ct, Question question, TestSubmissionDto submission)
    {
        var maxPoints = question.Weight;
        
        switch (question.Type)
        {
            case QuestionType.MultipleChoice:
            case QuestionType.TrueFalse:
                return ProcessMultipleChoiceAnswer(question, submission);
                
            case QuestionType.MultipleSelect:
                return ProcessMultipleSelectAnswer(question, submission);
                
            case QuestionType.ShortAnswer:
            case QuestionType.FillInTheBlank:
                return ProcessTextAnswer(question, submission);
                
            case QuestionType.Essay:
                return ProcessEssayAnswer(question, submission);
                
            case QuestionType.Numerical:
                return ProcessNumericalAnswer(question, submission);
                
            case QuestionType.Matching:
                return ProcessMatchingAnswer(question, submission);
                
            case QuestionType.Ordering:
                return ProcessOrderingAnswer(question, submission);
                
            case QuestionType.Scale:
                return ProcessScaleAnswer(question, submission);
                
            case QuestionType.FileUpload:
                return ProcessFileUploadAnswer(question, submission);
                
            default:
                return (false, 0, string.Empty, false);
        }
    }

    private (bool IsCorrect, double Points, string Answer, bool RequiresManualGrading) ProcessMultipleChoiceAnswer(
        Question question, TestSubmissionDto submission)
    {
        if (!submission.Answers.TryGetValue(question.Id, out var answer))
            return (false, 0, string.Empty, false);

        if (!Guid.TryParse(answer, out var selectedAnswerId))
            return (false, 0, answer, false);

        var correctAnswer = question.Answers.FirstOrDefault(a => a.Id == selectedAnswerId);
        if (correctAnswer == null)
            return (false, 0, answer, false);

        var isCorrect = correctAnswer.IsCorrect;
        var points = isCorrect ? correctAnswer.Points : 0;

        return (isCorrect, points, answer, false);
    }

    private (bool IsCorrect, double Points, string Answer, bool RequiresManualGrading) ProcessMultipleSelectAnswer(
        Question question, TestSubmissionDto submission)
    {
        if (!submission.MultipleSelectAnswers.TryGetValue(question.Id, out var selectedAnswerIds))
            return (false, 0, string.Empty, false);

        var correctAnswerIds = question.Answers.Where(a => a.IsCorrect).Select(a => a.Id).ToHashSet();
        var selectedSet = selectedAnswerIds.ToHashSet();

        var isCorrect = correctAnswerIds.SetEquals(selectedSet);
        var points = isCorrect ? question.Weight : 0;

        var answer = string.Join(",", selectedAnswerIds);
        return (isCorrect, points, answer, false);
    }

    private (bool IsCorrect, double Points, string Answer, bool RequiresManualGrading) ProcessTextAnswer(
        Question question, TestSubmissionDto submission)
    {
        if (!submission.Answers.TryGetValue(question.Id, out var answer))
            return (false, 0, string.Empty, false);

        var correctAnswers = question.Answers.Where(a => a.IsCorrect).ToList();
        if (!correctAnswers.Any())
            return (false, 0, answer, true); // Requires manual grading

        foreach (var correctAnswer in correctAnswers)
        {
            var isMatch = correctAnswer.IsCaseSensitive 
                ? answer.Equals(correctAnswer.Text, StringComparison.Ordinal)
                : answer.Equals(correctAnswer.Text, StringComparison.OrdinalIgnoreCase);

            if (isMatch)
                return (true, correctAnswer.Points, answer, false);

            // Check acceptable answers if defined
            if (!string.IsNullOrEmpty(correctAnswer.AcceptableAnswers))
            {
                try
                {
                    var acceptableAnswers = JsonSerializer.Deserialize<string[]>(correctAnswer.AcceptableAnswers);
                    if (acceptableAnswers != null)
                    {
                        foreach (var acceptable in acceptableAnswers)
                        {
                            var acceptableMatch = correctAnswer.IsCaseSensitive
                                ? answer.Equals(acceptable, StringComparison.Ordinal)
                                : answer.Equals(acceptable, StringComparison.OrdinalIgnoreCase);

                            if (acceptableMatch)
                                return (true, correctAnswer.Points, answer, false);
                        }
                    }
                }
                catch (JsonException)
                {
                    // Invalid JSON, continue with regular comparison
                }
            }
        }

        return (false, 0, answer, false);
    }

    private (bool IsCorrect, double Points, string Answer, bool RequiresManualGrading) ProcessEssayAnswer(
        Question question, TestSubmissionDto submission)
    {
        if (!submission.Answers.TryGetValue(question.Id, out var answer))
            return (false, 0, string.Empty, false);

        // Essay questions always require manual grading
        return (false, 0, answer, true);
    }

    private (bool IsCorrect, double Points, string Answer, bool RequiresManualGrading) ProcessNumericalAnswer(
        Question question, TestSubmissionDto submission)
    {
        if (!submission.NumericalAnswers.TryGetValue(question.Id, out var numericAnswer))
            return (false, 0, string.Empty, false);

        if (!question.CorrectNumericalAnswer.HasValue)
            return (false, 0, numericAnswer.ToString(), true);

        var tolerance = question.NumericalTolerance ?? 0;
        var correctValue = question.CorrectNumericalAnswer.Value;
        
        var isCorrect = Math.Abs(numericAnswer - correctValue) <= tolerance;
        var points = isCorrect ? question.Weight : 0;

        return (isCorrect, points, numericAnswer.ToString(), false);
    }

    private (bool IsCorrect, double Points, string Answer, bool RequiresManualGrading) ProcessMatchingAnswer(
        Question question, TestSubmissionDto submission)
    {
        if (!submission.MatchingAnswers.TryGetValue(question.Id, out var matchingAnswers))
            return (false, 0, string.Empty, false);

        var isCorrect = true;
        foreach (var pair in question.MatchPairs)
        {
            if (!matchingAnswers.TryGetValue(pair.LeftItemId, out var rightItemId) ||
                rightItemId != pair.RightItemId.ToString())
            {
                isCorrect = false;
                break;
            }
        }

        var points = isCorrect ? question.Weight : 0;
        var answer = JsonSerializer.Serialize(matchingAnswers);

        return (isCorrect, points, answer, false);
    }

    private (bool IsCorrect, double Points, string Answer, bool RequiresManualGrading) ProcessOrderingAnswer(
        Question question, TestSubmissionDto submission)
    {
        if (!submission.OrderingAnswers.TryGetValue(question.Id, out var orderedItems))
            return (false, 0, string.Empty, false);

        var correctOrder = question.OrderingItems.OrderBy(oi => oi.CorrectOrder).Select(oi => oi.Text).ToList();
        var isCorrect = orderedItems.SequenceEqual(correctOrder);
        var points = isCorrect ? question.Weight : 0;

        var answer = JsonSerializer.Serialize(orderedItems);
        return (isCorrect, points, answer, false);
    }

    private (bool IsCorrect, double Points, string Answer, bool RequiresManualGrading) ProcessScaleAnswer(
        Question question, TestSubmissionDto submission)
    {
        if (!submission.ScaleAnswers.TryGetValue(question.Id, out var scaleValue))
            return (false, 0, string.Empty, false);

        // Scale questions are typically used for surveys, not for scoring
        // But we can validate the range
        var isValid = question.ScaleMin.HasValue && question.ScaleMax.HasValue &&
                     scaleValue >= question.ScaleMin.Value && scaleValue <= question.ScaleMax.Value;

        var points = isValid ? question.Weight : 0;
        return (isValid, points, scaleValue.ToString(), false);
    }

    private (bool IsCorrect, double Points, string Answer, bool RequiresManualGrading) ProcessFileUploadAnswer(
        Question question, TestSubmissionDto submission)
    {
        if (!submission.FileSubmissions.TryGetValue(question.Id, out var filePath))
            return (false, 0, string.Empty, false);

        // File uploads always require manual review
        return (false, 0, filePath, true);
    }

    private string CalculateGrade(GradingScheme gradingScheme, int scorePercentage)
    {
        return gradingScheme switch
        {
            GradingScheme.PassFail => scorePercentage >= 70 ? "Pass" : "Fail",
            GradingScheme.LetterGrade => scorePercentage switch
            {
                >= 90 => "A",
                >= 80 => "B",
                >= 70 => "C",
                >= 60 => "D",
                _ => "F"
            },
            GradingScheme.Percentage => $"{scorePercentage}%",
            _ => scorePercentage.ToString()
        };
    }

    public async Task<ValidationResultDto> ValidateTestSubmissionAsync(CancellationToken ct, TestSubmissionDto submission)
    {
        var errors = new List<ValidationErrorDto>();

        var test = await _tsDbContext.Tests
            .Include(t => t.Questions)
            .FirstOrDefaultAsync(t => t.Id == submission.TestId, ct);

        if (test == null)
        {
            errors.Add(new ValidationErrorDto("TestId", "Test not found", "TEST_NOT_FOUND"));
            return new ValidationResultDto(false, errors);
        }

        // Validate required questions are answered
        foreach (var question in test.Questions.Where(q => q.IsRequired))
        {
            var hasAnswer = question.Type switch
            {
                QuestionType.MultipleChoice or QuestionType.TrueFalse or QuestionType.ShortAnswer or 
                QuestionType.Essay or QuestionType.FillInTheBlank => 
                    submission.Answers.ContainsKey(question.Id) && !string.IsNullOrEmpty(submission.Answers[question.Id]),
                
                QuestionType.MultipleSelect => 
                    submission.MultipleSelectAnswers.ContainsKey(question.Id) && submission.MultipleSelectAnswers[question.Id].Any(),
                
                QuestionType.Numerical => 
                    submission.NumericalAnswers.ContainsKey(question.Id),
                
                QuestionType.Scale => 
                    submission.ScaleAnswers.ContainsKey(question.Id),
                
                QuestionType.Matching => 
                    submission.MatchingAnswers.ContainsKey(question.Id) && submission.MatchingAnswers[question.Id].Any(),
                
                QuestionType.Ordering => 
                    submission.OrderingAnswers.ContainsKey(question.Id) && submission.OrderingAnswers[question.Id].Any(),
                
                QuestionType.FileUpload => 
                    submission.FileSubmissions.ContainsKey(question.Id) && !string.IsNullOrEmpty(submission.FileSubmissions[question.Id]),
                
                _ => false
            };

            if (!hasAnswer)
            {
                errors.Add(new ValidationErrorDto(
                    $"Question_{question.Id}", 
                    "This question is required", 
                    "REQUIRED_QUESTION"));
            }
        }

        return new ValidationResultDto(!errors.Any(), errors);
    }

    public async Task SaveProgressAsync(CancellationToken ct, Guid attemptId, Dictionary<Guid, string> answers)
    {
        var attempt = await _tsDbContext.TestAttempts.FindAsync(attemptId);
        if (attempt == null)
            throw new ArgumentException("Test attempt not found");

        // Store progress in ProctorData as JSON for now
        // In a production system, you might want a separate table for this
        var progressData = new { Answers = answers, SavedAt = DateTime.UtcNow };
        attempt.ProctorData = JsonSerializer.Serialize(progressData);
        attempt.UpdatedOn = DateTime.UtcNow;

        await _testRepository.UpdateTestAttemptAsync(ct, attempt);
    }

    public async Task<TestAttempt> UpdateTestAttemptAsync(CancellationToken ct, TestAttempt attempt)
    {
        return await _testRepository.UpdateTestAttemptAsync(ct, attempt);
    }

    public async Task AbandonTestAttemptAsync(CancellationToken ct, Guid attemptId)
    {
        var attempt = await _tsDbContext.TestAttempts.FindAsync(attemptId);
        if (attempt == null)
            throw new ArgumentException("Test attempt not found");

        attempt.IsAbandoned = true;
        attempt.UpdatedOn = DateTime.UtcNow;

        await _testRepository.UpdateTestAttemptAsync(ct, attempt);
    }

    public async Task<double> CalculateTestScoreAsync(CancellationToken ct, Test test, TestSubmissionDto submission)
    {
        double totalPoints = 0;
        double earnedPoints = 0;

        foreach (var question in test.Questions)
        {
            totalPoints += question.Weight;
            var (_, points, _, _) = await ProcessQuestionAnswerAsync(ct, question, submission);
            earnedPoints += points;
        }

        return totalPoints > 0 ? (earnedPoints / totalPoints) * 100 : 0;
    }

    public async Task<bool> ValidateAnswerAsync(CancellationToken ct, Question question, string answer)
    {
        // This would implement answer validation logic for individual questions
        // Used for real-time validation during test taking
        return true; // Simplified implementation
    }

    // Additional service methods would be implemented here following similar patterns
    public async Task<Test> CreateTestAsync(CancellationToken ct, CreateTestDto testDto)
    {
        var test = new Test
        {
            Id = Guid.NewGuid(),
            Name = testDto.Name,
            Description = testDto.Description,
            Instructions = testDto.Instructions,
            CompanyId = testDto.CompanyId,
            StartDate = testDto.StartDate,
            EndDate = testDto.EndDate,
            Duration = testDto.Duration,
            PassMark = testDto.PassMark,
            IsTimed = testDto.IsTimed,
            ShuffleQuestions = testDto.ShuffleQuestions,
            MaximumAttempts = testDto.MaximumAttempts,
            Visibility = Enum.Parse<TestVisibility>(testDto.Visibility),
            TestType = Enum.Parse<TestType>(testDto.TestType),
            Feedback = Enum.Parse<FeedbackType>(testDto.Feedback),
            TestAccessControl = Enum.Parse<AccessControl>(testDto.TestAccessControl),
            GradingScheme = Enum.Parse<GradingScheme>(testDto.GradingScheme),
            // Map other properties...
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        // Create RetakePolicy
        test.RetakePolicy = new RetakePolicy
        {
            AllowRetakes = testDto.RetakePolicy.AllowRetakes,
            MaxRetakes = testDto.RetakePolicy.MaxRetakes,
            RetakeInterval = testDto.RetakePolicy.RetakeInterval,
            RequirePasswordForRetake = testDto.RetakePolicy.RequirePasswordForRetake,
            ResetProgressOnRetake = testDto.RetakePolicy.ResetProgressOnRetake,
            ShowPreviousResults = testDto.RetakePolicy.ShowPreviousResults,
            RetakePenalty = testDto.RetakePolicy.RetakePenalty
        };

        return await _testRepository.CreateTestAsync(ct, test);
    }

    public async Task<Test> UpdateTestAsync(CancellationToken ct, Guid testId, CreateTestDto testDto)
    {
        var test = await _testRepository.GetTestByIdAsync(ct, testId);
        if (test == null)
            throw new ArgumentException("Test not found");

        // Update test properties
        test.Name = testDto.Name;
        test.Description = testDto.Description;
        test.Instructions = testDto.Instructions;
        // ... update other properties
        test.UpdatedOn = DateTime.UtcNow;

        return await _testRepository.UpdateTestAsync(ct, test);
    }

    public async Task<bool> DuplicateTestAsync(CancellationToken ct, Guid testId, string newName, Guid? targetCompanyId = null)
    {
        var originalTest = await _testRepository.GetTestByIdAsync(ct, testId);
        if (originalTest == null) return false;

        // Create a deep copy of the test
        var duplicatedTest = new Test
        {
            Id = Guid.NewGuid(),
            Name = newName,
            Description = originalTest.Description,
            Instructions = originalTest.Instructions,
            CompanyId = targetCompanyId ?? originalTest.CompanyId,
            // Copy all other properties...
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        // Copy questions and their related entities
        foreach (var originalQuestion in originalTest.Questions)
        {
            var duplicatedQuestion = new Question
            {
                Id = Guid.NewGuid(),
                TestId = duplicatedTest.Id,
                Text = originalQuestion.Text,
                Type = originalQuestion.Type,
                Weight = originalQuestion.Weight,
                // Copy all other properties...
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };

            // Copy answers
            foreach (var originalAnswer in originalQuestion.Answers)
            {
                duplicatedQuestion.Answers.Add(new Answer
                {
                    Id = Guid.NewGuid(),
                    QuestionId = duplicatedQuestion.Id,
                    Text = originalAnswer.Text,
                    IsCorrect = originalAnswer.IsCorrect,
                    // Copy all other properties...
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                });
            }

            // Copy match pairs and ordering items similarly...
            duplicatedTest.Questions.Add(duplicatedQuestion);
        }

        await _testRepository.CreateTestAsync(ct, duplicatedTest);
        return true;
    }

    public async Task<TestAnalyticsDto> GetTestAnalyticsAsync(CancellationToken ct, Guid testId)
    {
        return await _testRepository.GetTestAnalyticsAsync(ct, testId);
    }

    public async Task<byte[]> ExportTestResultsAsync(CancellationToken ct, Guid testId, string format = "csv")
    {
        var results = await _testRepository.GetTestResultsAsync(ct, testId);
        
        // Implementation would depend on format (CSV, Excel, PDF, etc.)
        // This is a simplified version
        var csv = "User,Score,CompletedDate,TimeSpent,Passed\n";
        foreach (var result in results)
        {
            csv += $"{result.UserId},{result.Score},{result.CompletedDate},{result.TimeSpent},{result.Passed}\n";
        }
        
        return Encoding.UTF8.GetBytes(csv);
    }

    public async Task<string> GenerateCertificateAsync(CancellationToken ct, Guid testResultId)
    {
        var result = await _testRepository.GetTestResultByIdAsync(ct, testResultId);
        if (result == null || !result.Passed)
            throw new ArgumentException("Test result not found or test not passed");

        // Certificate generation logic would go here
        // This could involve PDF generation, template processing, etc.
        var certificateUrl = $"/certificates/{testResultId}.pdf";
        
        result.CertificateUrl = certificateUrl;
        await _testRepository.UpdateTestResultAsync(ct, result);
        
        return certificateUrl;
    }

    public async Task<Test> ImportTestAsync(CancellationToken ct, TestImportDto importDto)
    {
        // Implementation would depend on the import format
        // Could support QTI, GIFT, JSON, XML, etc.
        throw new NotImplementedException("Test import functionality to be implemented");
    }

    public async Task<string> ExportTestAsync(CancellationToken ct, TestExportDto exportDto)
    {
        // Implementation would generate test export in requested format
        throw new NotImplementedException("Test export functionality to be implemented");
    }
}