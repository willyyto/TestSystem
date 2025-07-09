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
            company.Description,
            company.Website,
            company.LogoUrl,
            company.Address,
            company.City,
            company.State,
            company.Country,
            company.PostalCode,
            company.Phone,
            company.Email,
            company.ContactPerson,
            company.SubscriptionTier,
            company.SubscriptionStart,
            company.SubscriptionEnd,
            company.MaxUsers,
            company.MaxTests,
            company.MaxQuestionsPerTest,
            company.CustomBrandingEnabled,
            company.AdvancedReportsEnabled,
            company.ApiAccessEnabled,
            company.StorageLimitMB,
            company.StorageUsedMB,
            company.CustomDomain,
            company.IsActive,
            company.IsArchived
        );
    }

    public static Company MapToCompany(this AddCompanyDto dto)
    {
        return new Company
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            SubscriptionTier = "Free",
            MaxUsers = 10,
            MaxTests = 5,
            MaxQuestionsPerTest = 50,
            StorageLimitMB = 100,
            StorageUsedMB = 0,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };
    }
}