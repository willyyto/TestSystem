using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface IQuestionRepository
{
    Task<IEnumerable<Question>> GetQuestionsAsync(CancellationToken ct);
    Task<Question> AddQuestionAsync(CancellationToken ct, Question question);
    Task<Question> GetQuestionByIdAsync(CancellationToken ct, Guid id);
}