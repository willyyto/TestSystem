using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Mappers;

namespace userSystem.Mappers;

public static class UserMapper
{
    public static UserDto MapToUserDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.Username,
            user.Name,
            user.Email,
            user.Role,
            user.Company?.MapToCompanyDto(),
            user.IsArchived,
            user.IsActive,
            user.IsLocked
        );
    }
}