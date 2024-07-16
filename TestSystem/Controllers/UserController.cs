using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;
using TestSystem.Infra.Interfaces;
using userSystem.Mappers;

namespace TestSystem.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ICancellationTokenAccessor _cancellationTokenAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;

    public UserController(IUserRepository userRepository,
        ICancellationTokenAccessor cancellationTokenAccessor, IUserService userService)
    {
        _userRepository = userRepository;
        _cancellationTokenAccessor = cancellationTokenAccessor;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        var ct = _cancellationTokenAccessor.Token;
        var users = await _userRepository.GetAllUsersAsync(ct);
        return Ok(users.Select(i => i.MapToUserDto()).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser()
    {
        var ct = _cancellationTokenAccessor.Token;
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var user = await _userRepository.GetById(ct, Guid.Parse(userId));

        if (user == null) return NotFound();

        return Ok(user.MapToUserDto());
    }

    [HttpPost]
    public async Task<ActionResult<Guid?>> AddUser(AddUserDto User)
    {
        var ct = _cancellationTokenAccessor.Token;
        var id = await _userService.AddUserAsync(ct, User);
        return Ok(id);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Guid?>> DeleteUser(Guid id)
    {
        var ct = _cancellationTokenAccessor.Token;
        var user = await _userRepository.DeleteUserAsync(ct, id);
        if (user == null) return NotFound();
        return Ok(user);
    }
}