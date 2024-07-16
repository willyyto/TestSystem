namespace TestSystem.Core.Dtos;

public record UserDto(
    Guid Id,
    string Username,
    string Name,
    string Email,
    string Role,
    CompanyDto? Company,
    bool IsArchived,
    bool IsActive,
    bool Islocked);