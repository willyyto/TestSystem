namespace TestSystem.Core.Dtos;

public record UpdateUserDto(
    string Name,
    string Email,
    string Role,
    string? FirstName,
    string? LastName,
    string? Phone,
    string? Department,
    string? JobTitle,
    bool IsActive,
    bool IsLocked
);