using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Entities;
using TestSystem.Infra.Interfaces;

namespace TestSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository,
        ICancellationTokenAccessor cancellationTokenAccessor)
    {
        _userRepository = userRepository;
        _cancellationTokenAccessor = cancellationTokenAccessor;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        var ct = _cancellationTokenAccessor.Token;
        var user = await _userRepository.GetAllUsersAsync(ct);
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<Guid?>> AddUser(User User)
    {
        var ct = _cancellationTokenAccessor.Token;
        var id = await _userRepository.AddUserAsync(ct, User);
        return Ok(id);
    }
}