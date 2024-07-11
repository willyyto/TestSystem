using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public AdminCompanyController(ICompanyRepository CompanyRepository,
        ICancellationTokenAccessor cancellationTokenAccessor)
    {
        _CompanyRepository = CompanyRepository;
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
    
    [HttpPost()]
    public async Task<ActionResult<Company>> AddCompany(Company company)
    {
        var ct = _cancellationTokenAccessor.Token;
        
        var Company = await _CompanyRepository.AddCompanyAsync(ct, company);
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