using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace AuthenticationService.Models
{
    public class JWTContainerModel : IAuthContainerModel
    {
        public int ExpireMinutes { get; set; } = 10080;//7 days
        public string SecretKey { get; set; } = "d3d3Lm51cnVsaGlkYXlhdC5jb20=";
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;

        public Claim[] Claims { get; set; }
    }
}
