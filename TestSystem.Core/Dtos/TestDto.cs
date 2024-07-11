using System.Runtime.InteropServices.JavaScript;
using TestSystem.Core.Entities;

namespace TestSystem.Core.Dtos;

public record TestDto(Guid Id, string Title, string Company,DateTime StartDate, DateTime EndDate, List<QuestionDto> Questions, bool IsArchived, bool IsActive);