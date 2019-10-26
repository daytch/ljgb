using Microsoft.AspNetCore.Identity;

namespace ljgb.Common.Requests
{
    public class UserRequest
    {
        public IdentityUser user { get; set; }
        public string password { get; set; }

        public string HTMLTag { get; set; }
        public string EmailSubject { get; set; }
        public bool RememberMe { get; set; }
        public bool lockoutOnFailure { get; set; }
    }
}
