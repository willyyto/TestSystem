using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface IUserRepository
{
    // Basic CRUD
    Task<List<User>> GetAllUsersAsync(CancellationToken ct, Guid? companyId = null);
    Task<User?> GetByIdAsync(CancellationToken ct, Guid id);
    Task<User?> GetByUsernameAsync(CancellationToken ct, string username);
    Task<User?> GetByEmailAsync(CancellationToken ct, string email);
    Task<Guid> CreateUserAsync(CancellationToken ct, User user);
    Task<User> UpdateUserAsync(CancellationToken ct, User user);
    Task<User?> DeleteUserAsync(CancellationToken ct, Guid id);
    
    // Authentication and security
    Task<bool> ValidateUserCredentialsAsync(CancellationToken ct, string username, string password);
    Task UpdateLastLoginAsync(CancellationToken ct, Guid userId, string? ipAddress = null);
    Task<User?> GetByEmailVerificationTokenAsync(CancellationToken ct, string token);
    Task<User?> GetByPasswordResetTokenAsync(CancellationToken ct, string token);
    
    // Search and filtering
    Task<PagedResultDto<User>> SearchUsersAsync(CancellationToken ct, UserSearchDto searchDto);
    Task<IEnumerable<User>> GetUsersByRoleAsync(CancellationToken ct, string role, Guid? companyId = null);
    Task<IEnumerable<User>> GetInactiveUsersAsync(CancellationToken ct, DateTime since);
    
    // Bulk operations
    Task<IEnumerable<User>> BulkCreateUsersAsync(CancellationToken ct, IEnumerable<User> users);
    Task BulkUpdateUserStatusAsync(CancellationToken ct, IEnumerable<Guid> userIds, bool isActive);
    
    // Statistics
    Task<int> GetUserCountAsync(CancellationToken ct, Guid? companyId = null);
    Task<DashboardStatsDto> GetUserDashboardStatsAsync(CancellationToken ct, Guid? companyId = null);
}