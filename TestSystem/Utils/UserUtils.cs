using System.Security.Claims;

namespace TestSystem.Utils;

public static class UserUtils
{
    public static Guid GetUserId(ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User ID is missing in the token.");
        }

        return Guid.Parse(userId);
    }
}
