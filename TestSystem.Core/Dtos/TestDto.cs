using System.Runtime.InteropServices.JavaScript;
using TestSystem.Core.Entities;

namespace TestSystem.Core.Dtos;

public record TestDto(Guid Id, string Name, string Company, DateTime StartDate, DateTime EndDate, TimeSpan Duration, int PassMark, bool IsTimed, bool ShuffleQuestions, int MaximumAttempts, string Visibility, string TestType, string Instructions, string Feedback, string TestAccessControl, string GradingScheme, RetakePolicyDto RetakePolicy, List<QuestionDto> Questions, bool IsArchived, bool IsActive);
