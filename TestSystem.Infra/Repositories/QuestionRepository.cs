using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestSystem.Core.Entities;
using TestSystem.Infra.DataServices;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Infra.Repositories;

[InstanceScopedService]
public class QuestionRepository : IQuestionRepository
{
    private readonly ILogger<QuestionRepository> _logger;
    private readonly ITestSystemDbContextAsync _tsDbContext;

    public QuestionRepository(ITestSystemDbContextAsync tsDbContext, ILogger<QuestionRepository> logger)
    {
        _tsDbContext = tsDbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<Question>> GetQuestionsAsync(CancellationToken ct)
    {
        return await _tsDbContext.Questions.Include(q => q.Answers).ToListAsync(ct);
    }

    public async Task<Question> AddQuestionAsync(CancellationToken ct, Question question)
    {
        _tsDbContext.Questions.Add(question);
        await _tsDbContext.SaveChangesAsync(ct);
        return question;
    }

    public async Task<Question> GetQuestionByIdAsync(CancellationToken ct, Guid id)
    {
        return await _tsDbContext.Questions.Include(q => q.Answers).SingleOrDefaultAsync(q => q.Id == id);
    }
}