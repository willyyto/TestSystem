using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetCompaniesAsync(CancellationToken ct);
    Task<Company> GetCompanyByIdAsync(CancellationToken ct, Guid id);
    Task<Company> AddCompanyAsync(CancellationToken ct, Company company);
    Task<Company?> DeleteCompanyByIdAsync(CancellationToken ct, Guid id);
}