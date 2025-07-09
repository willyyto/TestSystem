namespace TestSystem.Core.Dtos;

public record ValidationResultDto(
    bool IsValid,
    List<ValidationErrorDto> Errors
);