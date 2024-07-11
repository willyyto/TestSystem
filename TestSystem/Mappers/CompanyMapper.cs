using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Mappers;

public static class CompanyMapper
{
    public static CompanyDto MapToCompanyDto(this Company company)
    {
        return new CompanyDto(
            company.Id,
            company.Name,
            company.IsActive,
            company.IsArchived
        );
    }
}