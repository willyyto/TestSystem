using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Infra.DataServices;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Infra.Repositories;

[InstanceScopedService]
public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITestSystemDbContextAsync _tsDbContext;

    public UserRepository(
        ITestSystemDbContextAsync tsDbContext, 
        ILogger<UserRepository> logger,
        IPasswordHasher<User> passwordHasher)
    {
        _tsDbContext = tsDbContext;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    #region Basic CRUD Operations

    public async Task<List<User>> GetAllUsersAsync(CancellationToken ct, Guid? companyId = null)
    {
        var query = _tsDbContext.Users.Include(u => u.Company).AsQueryable();
        
        if (companyId.HasValue)
            query = query.Where(u => u.CompanyId == companyId.Value);
            
        return await query
            .Where(u => !u.IsArchived)
            .OrderBy(u => u.Name)
            .ToListAsync(ct);
    }

    public async Task<User?> GetByIdAsync(CancellationToken ct, Guid id)
    {
        return await _tsDbContext.Users
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsArchived, ct);
    }

    public async Task<User?> GetByUsernameAsync(CancellationToken ct, string username)
    {
        return await _tsDbContext.Users
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.Username == username && !u.IsArchived, ct);
    }

    public async Task<User?> GetByEmailAsync(CancellationToken ct, string email)
    {
        return await _tsDbContext.Users
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsArchived, ct);
    }

    public async Task<Guid> CreateUserAsync(CancellationToken ct, User user)
    {
        try
        {
            // Don't hash password here if it's already hashed
            // The UserService should handle password hashing
            user.CreatedOn = DateTime.UtcNow;
            user.UpdatedOn = DateTime.UtcNow;

            await _tsDbContext.Users.AddAsync(user, ct);
            await _tsDbContext.SaveChangesAsync(ct);
            
            _logger.LogInformation("Created user {Username} with ID {UserId}", user.Username, user.Id);
            return user.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user {Username}", user.Username);
            throw;
        }
    }

    public async Task<User> UpdateUserAsync(CancellationToken ct, User user)
    {
        try
        {
            user.UpdatedOn = DateTime.UtcNow;
            _tsDbContext.Users.Update(user);
            await _tsDbContext.SaveChangesAsync(ct);
            
            _logger.LogInformation("Updated user {Username} with ID {UserId}", user.Username, user.Id);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user {UserId}", user.Id);
            throw;
        }
    }

    public async Task<User?> DeleteUserAsync(CancellationToken ct, Guid id)
    {
        var user = await GetByIdAsync(ct, id);
        if (user == null) return null;

        try
        {
            // Soft delete - mark as archived
            user.IsArchived = true;
            user.IsActive = false;
            user.UpdatedOn = DateTime.UtcNow;
            
            await UpdateUserAsync(ct, user);
            
            _logger.LogInformation("Soft deleted user {Username} with ID {UserId}", user.Username, user.Id);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete user {UserId}", id);
            throw;
        }
    }

    #endregion

    #region Authentication and Security

    public async Task<bool> ValidateUserCredentialsAsync(CancellationToken ct, string username, string password)
    {
        var user = await GetByUsernameAsync(ct, username);
        if (user == null || !user.IsActive || user.IsLocked) 
            return false;

        try
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            var isValid = result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
            
            // If SuccessRehashNeeded, update the password hash
            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.Password = _passwordHasher.HashPassword(user, password);
                await UpdateUserAsync(ct, user);
                _logger.LogInformation("Rehashed password for user {Username}", username);
            }
            
            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating credentials for user {Username}", username);
            return false;
        }
    }

    public async Task UpdateLastLoginAsync(CancellationToken ct, Guid userId, string? ipAddress = null)
    {
        var user = await _tsDbContext.Users.FindAsync(userId);
        if (user == null) return;

        user.LastLoginAt = DateTime.UtcNow;
        user.LastLoginIp = ipAddress;
        user.UpdatedOn = DateTime.UtcNow;

        await _tsDbContext.SaveChangesAsync(ct);
    }

    public async Task<User?> GetByEmailVerificationTokenAsync(CancellationToken ct, string token)
    {
        return await _tsDbContext.Users
            .FirstOrDefaultAsync(u => u.EmailVerificationToken == token && 
                                    u.EmailVerificationExpires > DateTime.UtcNow &&
                                    !u.IsArchived, ct);
    }

    public async Task<User?> GetByPasswordResetTokenAsync(CancellationToken ct, string token)
    {
        return await _tsDbContext.Users
            .FirstOrDefaultAsync(u => u.PasswordResetToken == token && 
                                    u.PasswordResetExpires > DateTime.UtcNow &&
                                    !u.IsArchived, ct);
    }

    #endregion

    #region Password Management

    public async Task<bool> UpdatePasswordAsync(CancellationToken ct, Guid userId, string currentPassword, string newPassword)
    {
        var user = await GetByIdAsync(ct, userId);
        if (user == null) return false;

        // Verify current password
        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, currentPassword);
        if (verificationResult == PasswordVerificationResult.Failed)
            return false;

        // Hash and set new password
        user.Password = _passwordHasher.HashPassword(user, newPassword);
        user.UpdatedOn = DateTime.UtcNow;

        await UpdateUserAsync(ct, user);
        _logger.LogInformation("Password updated for user {UserId}", userId);
        
        return true;
    }

    public async Task<bool> ResetPasswordAsync(CancellationToken ct, string token, string newPassword)
    {
        var user = await GetByPasswordResetTokenAsync(ct, token);
        if (user == null) return false;

        // Hash and set new password
        user.Password = _passwordHasher.HashPassword(user, newPassword);
        user.PasswordResetToken = null;
        user.PasswordResetExpires = null;
        user.UpdatedOn = DateTime.UtcNow;

        await UpdateUserAsync(ct, user);
        _logger.LogInformation("Password reset for user {UserId}", user.Id);
        
        return true;
    }

    #endregion

    #region Search and Filtering

    public async Task<PagedResultDto<User>> SearchUsersAsync(CancellationToken ct, UserSearchDto searchDto)
    {
        var query = _tsDbContext.Users
            .Include(u => u.Company)
            .Where(u => !u.IsArchived);

        // Apply filters
        if (!string.IsNullOrEmpty(searchDto.SearchTerm))
        {
            var term = searchDto.SearchTerm.ToLower();
            query = query.Where(u => u.Name.ToLower().Contains(term) ||
                                   u.Username.ToLower().Contains(term) ||
                                   u.Email.ToLower().Contains(term));
        }

        if (searchDto.Roles?.Any() == true)
        {
            query = query.Where(u => searchDto.Roles.Contains(u.Role));
        }

        if (searchDto.CompanyId.HasValue)
        {
            query = query.Where(u => u.CompanyId == searchDto.CompanyId.Value);
        }

        if (searchDto.IsActive.HasValue)
        {
            query = query.Where(u => u.IsActive == searchDto.IsActive.Value);
        }

        if (searchDto.EmailVerified.HasValue)
        {
            query = query.Where(u => u.EmailVerified == searchDto.EmailVerified.Value);
        }

        if (searchDto.LastLoginAfter.HasValue)
        {
            query = query.Where(u => u.LastLoginAt > searchDto.LastLoginAfter.Value);
        }

        // Apply sorting
        query = searchDto.SortBy?.ToLower() switch
        {
            "name" => searchDto.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(u => u.Name)
                : query.OrderBy(u => u.Name),
            "username" => searchDto.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(u => u.Username)
                : query.OrderBy(u => u.Username),
            "email" => searchDto.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(u => u.Email)
                : query.OrderBy(u => u.Email),
            "createdon" => searchDto.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(u => u.CreatedOn)
                : query.OrderBy(u => u.CreatedOn),
            "lastlogin" => searchDto.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(u => u.LastLoginAt)
                : query.OrderBy(u => u.LastLoginAt),
            _ => query.OrderBy(u => u.Name)
        };

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((searchDto.Page - 1) * searchDto.PageSize)
            .Take(searchDto.PageSize)
            .ToListAsync(ct);

        return new PagedResultDto<User>(
            items,
            totalCount,
            searchDto.Page,
            searchDto.PageSize,
            (int)Math.Ceiling((double)totalCount / searchDto.PageSize)
        );
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(CancellationToken ct, string role, Guid? companyId = null)
    {
        var query = _tsDbContext.Users
            .Include(u => u.Company)
            .Where(u => u.Role == role && !u.IsArchived);

        if (companyId.HasValue)
            query = query.Where(u => u.CompanyId == companyId.Value);

        return await query.ToListAsync(ct);
    }

    public async Task<IEnumerable<User>> GetInactiveUsersAsync(CancellationToken ct, DateTime since)
    {
        return await _tsDbContext.Users
            .Include(u => u.Company)
            .Where(u => u.LastLoginAt < since && !u.IsArchived)
            .ToListAsync(ct);
    }

    #endregion

    #region Bulk Operations

    public async Task<IEnumerable<User>> BulkCreateUsersAsync(CancellationToken ct, IEnumerable<User> users)
    {
        var userList = users.ToList();
        
        try
        {
            foreach (var user in userList)
            {
                // Ensure password is hashed
                if (!string.IsNullOrEmpty(user.Password) && !user.Password.StartsWith("$2"))
                {
                    user.Password = _passwordHasher.HashPassword(user, user.Password);
                }
                user.CreatedOn = DateTime.UtcNow;
                user.UpdatedOn = DateTime.UtcNow;
            }

            await _tsDbContext.Users.AddRangeAsync(userList, ct);
            await _tsDbContext.SaveChangesAsync(ct);
            
            _logger.LogInformation("Bulk created {Count} users", userList.Count);
            return userList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to bulk create {Count} users", userList.Count);
            throw;
        }
    }

    public async Task BulkUpdateUserStatusAsync(CancellationToken ct, IEnumerable<Guid> userIds, bool isActive)
    {
        var users = await _tsDbContext.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync(ct);

        foreach (var user in users)
        {
            user.IsActive = isActive;
            user.UpdatedOn = DateTime.UtcNow;
        }

        await _tsDbContext.SaveChangesAsync(ct);
        _logger.LogInformation("Bulk updated status for {Count} users to {Status}", users.Count, isActive);
    }

    #endregion

    #region Statistics

    public async Task<int> GetUserCountAsync(CancellationToken ct, Guid? companyId = null)
    {
        var query = _tsDbContext.Users.Where(u => !u.IsArchived);
        
        if (companyId.HasValue)
            query = query.Where(u => u.CompanyId == companyId.Value);
            
        return await query.CountAsync(ct);
    }

    public async Task<DashboardStatsDto> GetUserDashboardStatsAsync(CancellationToken ct, Guid? companyId = null)
    {
        var query = _tsDbContext.Users.AsQueryable();
        
        if (companyId.HasValue)
            query = query.Where(u => u.CompanyId == companyId.Value);

        var totalUsers = await query.CountAsync(u => !u.IsArchived, ct);
        var activeUsers = await query.CountAsync(u => u.IsActive && !u.IsArchived, ct);
        var recentLogins = await query.CountAsync(u => u.LastLoginAt > DateTime.UtcNow.AddDays(-7), ct);

        // Get recent activity
        var recentActivity = await _tsDbContext.Users
            .Where(u => u.CreatedOn > DateTime.UtcNow.AddDays(-30))
            .OrderByDescending(u => u.CreatedOn)
            .Take(10)
            .Select(u => new RecentActivityDto(
                "UserRegistration",
                $"User {u.Name} registered",
                u.CreatedOn,
                u.Id,
                u.Name
            ))
            .ToListAsync(ct);

        return new DashboardStatsDto(
            0, // TotalTests - would be populated by TestRepository
            0, // ActiveTests - would be populated by TestRepository  
            totalUsers,
            0, // TotalAttempts - would be populated by TestRepository
            recentLogins,
            0, // AverageScore - would be calculated across all tests
            recentActivity
        );
    }

    #endregion
}