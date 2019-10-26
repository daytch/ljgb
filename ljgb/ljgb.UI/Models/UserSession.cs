using System.Security.Claims;

namespace ljgb.UI.Models
{
    public static class UserSession
    {
        public static ClaimsPrincipal principal { get; set; }
        public static string Token { get; set; }
    }
}
