using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Infra.Interfaces;
using TestSystem.Mappers;

namespace TestSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AdminCompanyController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly ICompanyRepository _CompanyRepository;
    private readonly ICompanyService _companyService;

    public AdminCompanyController(ICompanyRepository CompanyRepository, ICompanyService companyService,
        ICancellationTokenAccessor cancellationTokenAccessor)
    {
        _CompanyRepository = CompanyRepository;
        _companyService = companyService;
        _cancellationTokenAccessor = cancellationTokenAccessor;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Company>>> GetCompanys()
    {
        var ct = _cancellationTokenAccessor.Token;
        var Companys = await _CompanyRepository.GetCompaniesAsync(ct);
        return Ok(Companys.Select(i => i.MapToCompanyDto()).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Company>> GetCompany(Guid id)
    {
        var ct = _cancellationTokenAccessor.Token;
        var Company = await _CompanyRepository.GetCompanyByIdAsync(ct, id);
        if (Company == null) return NotFound();
        return Ok(Company.MapToCompanyDto());
    }

    [HttpPost]
    public async Task<ActionResult<Company>> AddCompany(AddCompanyDto company)
    {
        var ct = _cancellationTokenAccessor.Token;
        var Company = await _companyService.AddCompanyAsync(ct, company);
        if (Company == null) return NotFound();
        return Ok(Company.MapToCompanyDto());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Company>> DeleteCompany(Guid id)
    {
        var ct = _cancellationTokenAccessor.Token;
        var Company = await _CompanyRepository.DeleteCompanyByIdAsync(ct, id);
        if (Company == null) return NotFound();
        return Ok(Company.MapToCompanyDto());
    }
}