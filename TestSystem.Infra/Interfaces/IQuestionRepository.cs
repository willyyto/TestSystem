using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface IQuestionRepository
{
    Task<IEnumerable<Question>> GetQuestionsAsync(CancellationToken ct, Guid? testId = null);
    Task<Question?> GetQuestionByIdAsync(CancellationToken ct, Guid id);
    Task<Question> CreateQuestionAsync(CancellationToken ct, Question question);
    Task<Question> UpdateQuestionAsync(CancellationToken ct, Question question);
    Task<bool> DeleteQuestionAsync(CancellationToken ct, Guid id);
    
    // Question with related data
    Task<Question?> GetQuestionWithAnswersAsync(CancellationToken ct, Guid id);
    Task<IEnumerable<Question>> GetQuestionsWithAnswersAsync(CancellationToken ct, Guid testId);
    
    // Ordering and management
    Task UpdateQuestionOrderAsync(CancellationToken ct, IEnumerable<(Guid Id, int Order)> questionOrders);
    Task<IEnumerable<Question>> GetRandomQuestionsAsync(CancellationToken ct, Guid testId, int count);
    
    // Import/Export
    Task<IEnumerable<Question>> BulkCreateQuestionsAsync(CancellationToken ct, IEnumerable<Question> questions);
    Task<Question> DuplicateQuestionAsync(CancellationToken ct, Guid questionId, Guid targetTestId);
}