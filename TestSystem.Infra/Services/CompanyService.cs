using Microsoft.Extensions.Configuration;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Infra.Services;

[InstanceScopedService]
public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IConfiguration _configuration;

    public CompanyService(IConfiguration configuration,
        ICompanyRepository companyRepository)
    {
        _configuration = configuration;
        _companyRepository = companyRepository;
    }

    public async Task<Company> AddCompanyAsync(CancellationToken ct, AddCompanyDto request)
    {
        var newcompany = new Company
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            IsActive = true
        };
        var company = await _companyRepository.CreateCompanyAsync(ct, newcompany);
        return company;
    }
}