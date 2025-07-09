using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Infra.DataServices;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Infra.Repositories;

[InstanceScopedService]
public class CompanyRepository : ICompanyRepository
{
    private readonly ILogger<CompanyRepository> _logger;
    private readonly ITestSystemDbContextAsync _tsDbContext;

    public CompanyRepository(ITestSystemDbContextAsync tsDbContext, ILogger<CompanyRepository> logger)
    {
        _tsDbContext = tsDbContext;
        _logger = logger;
    }

    #region Basic CRUD Operations

    public async Task<IEnumerable<Company>> GetCompaniesAsync(CancellationToken ct)
    {
        return await _tsDbContext.Companies
            .Where(c => !c.IsArchived)
            .OrderBy(c => c.Name)
            .ToListAsync(ct);
    }

    public async Task<Company?> GetCompanyByIdAsync(CancellationToken ct, Guid id)
    {
        return await _tsDbContext.Companies
            .Include(c => c.Users.Where(u => !u.IsArchived))
            .Include(c => c.Tests.Where(t => !t.IsArchived))
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsArchived, ct);
    }

    public async Task<Company> CreateCompanyAsync(CancellationToken ct, Company company)
    {
        try
        {
            company.CreatedOn = DateTime.UtcNow;
            company.UpdatedOn = DateTime.UtcNow;

            _tsDbContext.Companies.Add(company);
            await _tsDbContext.SaveChangesAsync(ct);
            
            _logger.LogInformation("Created company {CompanyName} with ID {CompanyId}", company.Name, company.Id);
            return company;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create company {CompanyName}", company.Name);
            throw;
        }
    }

    public async Task<Company> UpdateCompanyAsync(CancellationToken ct, Company company)
    {
        try
        {
            company.UpdatedOn = DateTime.UtcNow;
            _tsDbContext.Companies.Update(company);
            await _tsDbContext.SaveChangesAsync(ct);
            
            _logger.LogInformation("Updated company {CompanyName} with ID {CompanyId}", company.Name, company.Id);
            return company;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update company {CompanyId}", company.Id);
            throw;
        }
    }

    public async Task<Company?> DeleteCompanyByIdAsync(CancellationToken ct, Guid id)
    {
        var company = await GetCompanyByIdAsync(ct, id);
        if (company == null) return null;

        try
        {
            // Soft delete - mark as archived
            company.IsArchived = true;
            company.IsActive = false;
            company.UpdatedOn = DateTime.UtcNow;
            
            await UpdateCompanyAsync(ct, company);
            
            _logger.LogInformation("Soft deleted company {CompanyName} with ID {CompanyId}", company.Name, company.Id);
            return company;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete company {CompanyId}", id);
            throw;
        }
    }

    #endregion

    #region Enhanced Features

    public async Task<Company?> GetCompanyByDomainAsync(CancellationToken ct, string domain)
    {
        return await _tsDbContext.Companies
            .FirstOrDefaultAsync(c => c.CustomDomain == domain && !c.IsArchived, ct);
    }

    public async Task<bool> ValidateCompanyLimitsAsync(CancellationToken ct, Guid companyId)
    {
        var company = await _tsDbContext.Companies.FindAsync(companyId);
        if (company == null) return false;

        var userCount = await _tsDbContext.Users.CountAsync(u => u.CompanyId == companyId && !u.IsArchived, ct);
        var testCount = await _tsDbContext.Tests.CountAsync(t => t.CompanyId == companyId && !t.IsArchived, ct);

        return userCount <= company.MaxUsers && testCount <= company.MaxTests;
    }

    public async Task UpdateStorageUsageAsync(CancellationToken ct, Guid companyId, long usedMB)
    {
        var company = await _tsDbContext.Companies.FindAsync(companyId);
        if (company == null) return;

        company.StorageUsedMB = usedMB;
        company.UpdatedOn = DateTime.UtcNow;

        await _tsDbContext.SaveChangesAsync(ct);
    }

    public async Task<CompanySettingsDto> GetCompanySettingsAsync(CancellationToken ct, Guid companyId)
    {
        var company = await _tsDbContext.Companies.FindAsync(companyId);
        if (company == null) throw new ArgumentException("Company not found");

        // Parse JSON settings (simplified - you'd use proper JSON parsing)
        var emailSettings = !string.IsNullOrEmpty(company.SmtpSettings)
            ? JsonSerializer.Deserialize<EmailSettingsDto>(company.SmtpSettings)
            : null;

        return new CompanySettingsDto(
            companyId,
            company.CustomCss,
            company.CustomDomain,
            emailSettings,
            null, // SecuritySettings would be parsed from Settings JSON
            null  // BrandingSettings would be parsed from Settings JSON
        );
    }

    public async Task UpdateCompanySettingsAsync(CancellationToken ct, Guid companyId, CompanySettingsDto settings)
    {
        var company = await _tsDbContext.Companies.FindAsync(companyId);
        if (company == null) throw new ArgumentException("Company not found");

        company.CustomCss = settings.CustomCss;
        company.CustomDomain = settings.CustomDomain;
        
        if (settings.EmailSettings != null)
        {
            company.SmtpSettings = JsonSerializer.Serialize(settings.EmailSettings);
        }

        company.UpdatedOn = DateTime.UtcNow;
        await _tsDbContext.SaveChangesAsync(ct);
    }

    #endregion

    #region Subscription Management

    public async Task UpdateSubscriptionAsync(CancellationToken ct, Guid companyId, string tier, DateTime? start, DateTime? end)
    {
        var company = await _tsDbContext.Companies.FindAsync(companyId);
        if (company == null) throw new ArgumentException("Company not found");

        company.SubscriptionTier = tier;
        company.SubscriptionStart = start;
        company.SubscriptionEnd = end;
        
        // Update limits based on tier
        (company.MaxUsers, company.MaxTests, company.MaxQuestionsPerTest, company.StorageLimitMB) = tier switch
        {
            "Free" => (10, 5, 50, 100),
            "Pro" => (100, 50, 200, 1000),
            "Enterprise" => (1000, 500, 1000, 10000),
            _ => (10, 5, 50, 100)
        };

        company.CustomBrandingEnabled = tier != "Free";
        company.AdvancedReportsEnabled = tier == "Enterprise";
        company.ApiAccessEnabled = tier == "Enterprise";

        company.UpdatedOn = DateTime.UtcNow;
        await _tsDbContext.SaveChangesAsync(ct);
        
        _logger.LogInformation("Updated subscription for company {CompanyId} to {Tier}", companyId, tier);
    }

    public async Task<IEnumerable<Company>> GetExpiredSubscriptionsAsync(CancellationToken ct)
    {
        return await _tsDbContext.Companies
            .Where(c => c.SubscriptionEnd.HasValue && 
                       c.SubscriptionEnd.Value < DateTime.UtcNow &&
                       !c.IsArchived)
            .ToListAsync(ct);
    }

    #endregion
}