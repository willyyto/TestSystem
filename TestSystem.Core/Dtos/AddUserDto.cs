using TestSystem.Core.Entities;

namespace TestSystem.Core.Dtos;

public record AddUserDto(string Username, string Password, string Name, string Email, string Role, Guid CompanyId);