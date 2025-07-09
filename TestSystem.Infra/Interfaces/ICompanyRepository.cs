using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetCompaniesAsync(CancellationToken ct);
    Task<Company?> GetCompanyByIdAsync(CancellationToken ct, Guid id);
    Task<Company> CreateCompanyAsync(CancellationToken ct, Company company);
    Task<Company> UpdateCompanyAsync(CancellationToken ct, Company company);
    Task<Company?> DeleteCompanyByIdAsync(CancellationToken ct, Guid id);
    
    // Enhanced features
    Task<Company?> GetCompanyByDomainAsync(CancellationToken ct, string domain);
    Task<bool> ValidateCompanyLimitsAsync(CancellationToken ct, Guid companyId);
    Task UpdateStorageUsageAsync(CancellationToken ct, Guid companyId, long usedMB);
    Task<CompanySettingsDto> GetCompanySettingsAsync(CancellationToken ct, Guid companyId);
    Task UpdateCompanySettingsAsync(CancellationToken ct, Guid companyId, CompanySettingsDto settings);
    
    // Subscription management
    Task UpdateSubscriptionAsync(CancellationToken ct, Guid companyId, string tier, DateTime? start, DateTime? end);
    Task<IEnumerable<Company>> GetExpiredSubscriptionsAsync(CancellationToken ct);
}