using System.Security.Claims;

namespace API.Utility
{
    public class UserContext
    {
        public static string GetEmail(ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email) ?? "unknown";
        }
    }
}
