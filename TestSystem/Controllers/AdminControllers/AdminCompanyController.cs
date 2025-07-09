using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Extensions;
using TestSystem.Filters;
using TestSystem.Infra.Interfaces;
using TestSystem.Mappers;

namespace TestSystem.Controllers;

[Authorize(Roles = "Administrator")]
[ApiController]
[Route("api/admin/[controller]")]
public class AdminCompanyController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyService _companyService;
    private readonly ILogger<AdminCompanyController> _logger;

    public AdminCompanyController(
        ICompanyRepository companyRepository,
        ICompanyService companyService,
        ICancellationTokenAccessor cancellationTokenAccessor,
        ILogger<AdminCompanyController> logger)
    {
        _companyRepository = companyRepository;
        _companyService = companyService;
        _cancellationTokenAccessor = cancellationTokenAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Get all companies
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<List<CompanyDto>>), 200)]
    public async Task<IActionResult> GetCompanies()
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var companies = await _companyRepository.GetCompaniesAsync(ct);
            var companyDtos = companies.Select(c => c.MapToCompanyDto()).ToList();
            
            return this.OkResponse(companyDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving companies");
            return this.ExceptionResponse<List<CompanyDto>>(ex);
        }
    }

    /// <summary>
    /// Get company by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<CompanyDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var company = await _companyRepository.GetCompanyByIdAsync(ct, id);
            
            if (company == null)
                return this.NotFoundResponse<string>("Company not found");

            return this.OkResponse(company.MapToCompanyDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving company {CompanyId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Create a new company
    /// </summary>
    [HttpPost]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<CompanyDto>), 201)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 400)]
    public async Task<IActionResult> CreateCompany([FromBody] AddCompanyDto companyDto)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var company = await _companyService.AddCompanyAsync(ct, companyDto);
            
            var companyDtoResult = company.MapToCompanyDto();
            return this.CreatedResponse(nameof(GetCompany), new { id = company.Id }, 
                companyDtoResult, "Company created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating company");
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Update company
    /// </summary>
    [HttpPut("{id}")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<CompanyDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] UpdateCompanyDto companyDto)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var company = await _companyRepository.GetCompanyByIdAsync(ct, id);
            
            if (company == null)
                return this.NotFoundResponse<string>("Company not found");

            // Update company properties
            company.Name = companyDto.Name;
            company.Description = companyDto.Description;
            company.Website = companyDto.Website;
            company.LogoUrl = companyDto.LogoUrl;
            company.Address = companyDto.Address;
            company.City = companyDto.City;
            company.State = companyDto.State;
            company.Country = companyDto.Country;
            company.PostalCode = companyDto.PostalCode;
            company.Phone = companyDto.Phone;
            company.Email = companyDto.Email;
            company.ContactPerson = companyDto.ContactPerson;
            company.SubscriptionTier = companyDto.SubscriptionTier;
            company.MaxUsers = companyDto.MaxUsers;
            company.MaxTests = companyDto.MaxTests;
            company.MaxQuestionsPerTest = companyDto.MaxQuestionsPerTest;
            company.CustomBrandingEnabled = companyDto.CustomBrandingEnabled;
            company.AdvancedReportsEnabled = companyDto.AdvancedReportsEnabled;
            company.ApiAccessEnabled = companyDto.ApiAccessEnabled;
            company.StorageLimitMB = companyDto.StorageLimitMB;
            company.CustomDomain = companyDto.CustomDomain;
            company.UpdatedOn = DateTime.UtcNow;

            var updatedCompany = await _companyRepository.UpdateCompanyAsync(ct, company);
            return this.OkResponse(updatedCompany.MapToCompanyDto(), "Company updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating company {CompanyId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Delete company (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 404)]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var deletedCompany = await _companyRepository.DeleteCompanyByIdAsync(ct, id);
            
            if (deletedCompany == null)
                return this.NotFoundResponse<string>("Company not found");

            _logger.LogInformation("Company deleted: {CompanyId}", id);
            return this.OkResponse("Company deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting company {CompanyId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Get company settings
    /// </summary>
    [HttpGet("{id}/settings")]
    [ProducesResponseType(typeof(ApiResponseDto<CompanySettingsDto>), 200)]
    public async Task<IActionResult> GetCompanySettings(Guid id)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            var settings = await _companyRepository.GetCompanySettingsAsync(ct, id);
            
            return this.OkResponse(settings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving company settings {CompanyId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Update company settings
    /// </summary>
    [HttpPut("{id}/settings")]
    [ValidateModel]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    public async Task<IActionResult> UpdateCompanySettings(Guid id, [FromBody] CompanySettingsDto settings)
    {
        try
        {
            var ct = _cancellationTokenAccessor.Token;
            await _companyRepository.UpdateCompanySettingsAsync(ct, id, settings);
            
            return this.OkResponse("Settings updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating company settings {CompanyId}", id);
            return this.ExceptionResponse<string>(ex);
        }
    }
}
