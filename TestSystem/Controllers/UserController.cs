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
    public List<User> GetUsers()
    {
        var ct = _cancellationTokenAccessor.Token;
        var task = _userRepository.GetAllUsersAsync(ct);
        return task.Result.ToList();
    }

    [HttpPost]
    public Guid? AddUser(User User)
    {
        var ct = _cancellationTokenAccessor.Token;
        var task = _userRepository.AddUserAsync(ct, User);
        task.Wait(ct);
        return task.Result;
    }
}