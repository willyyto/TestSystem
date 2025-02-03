using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface ICompanyService
{
    Task<Company> AddCompanyAsync(CancellationToken ct, AddCompanyDto request);
}