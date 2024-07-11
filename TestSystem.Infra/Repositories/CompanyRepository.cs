using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

    public async Task<IEnumerable<Company>> GetCompaniesAsync(CancellationToken ct)
    {
        return await _tsDbContext.Companies.ToListAsync(ct);
    }

    public async Task<Company> AddCompanyAsync(CancellationToken ct, Company Company)
    {
        _tsDbContext.Companies.Add(Company);
        await _tsDbContext.SaveChangesAsync(ct);
        return Company;
    }

    public async Task<Company> GetCompanyByIdAsync(CancellationToken ct, Guid id)
    {
        return await _tsDbContext.Companies.SingleOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<Company?> DeleteCompanyByIdAsync(CancellationToken ct, Guid id)
    {
        var company = await _tsDbContext.Companies
            .FirstOrDefaultAsync(t => t.Id == id, ct);
        
        if (company == null) return null;
        
        _tsDbContext.Companies.Remove(company);
        await _tsDbContext.SaveChangesAsync(ct);
        
        return company;
    }
}