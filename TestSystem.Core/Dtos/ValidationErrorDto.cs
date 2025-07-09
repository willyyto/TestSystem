namespace TestSystem.Core.Dtos;

public record ValidationErrorDto(
    string Field,
    string Message,
    string Code
);