using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Infra.Interfaces;

public interface IUserService
{
     public RefreshToken GenerateRefreshToken();
     public string CreateToken(User user);
}