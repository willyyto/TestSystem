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
            user.FirstName,
            user.LastName,
            user.ProfilePictureUrl,
            user.Phone,
            user.Department,
            user.JobTitle,
            user.LastLoginAt,
            user.EmailVerified,
            user.TwoFactorEnabled,
            user.Timezone,
            user.Language,
            user.NotificationEmailEnabled,
            user.NotificationSmsEnabled,
            user.Company?.MapToCompanyDto(),
            user.IsArchived,
            user.IsActive,
            user.IsLocked
        );
    }

    public static User MapToUser(this AddUserDto dto)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Password = dto.Password, // Should be hashed before saving
            Name = dto.Name,
            Email = dto.Email,
            Role = dto.Role,
            CompanyId = dto.CompanyId,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            RefreshToken = string.Empty,
            TokenCreated = DateTime.UtcNow,
            TokenExpires = DateTime.UtcNow.AddDays(7)
        };
    }

    public static User MapToUser(this RegisterDto dto)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Password = dto.Password, // Should be hashed before saving
            Name = dto.Name ?? string.Empty,
            Email = dto.Email ?? string.Empty,
            Role = dto.Role ?? "User",
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            RefreshToken = string.Empty,
            TokenCreated = DateTime.UtcNow,
            TokenExpires = DateTime.UtcNow.AddDays(7)
        };
    }
}