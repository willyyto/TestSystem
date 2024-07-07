using TestSystem.Core.Dtos;

namespace TestSystem.Infra.Interfaces;

public interface ITestService
{
    Task<int?> SubmitQuiz(TestSubmissionDto submission);
}