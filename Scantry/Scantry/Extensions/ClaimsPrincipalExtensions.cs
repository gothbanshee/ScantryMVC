using System.Security.Claims;
using static Scantry.GCommon.Messages.GeneralMessages;

namespace Scantry.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? throw new InvalidOperationException(UserIdNotFound);
        }
    }
}
