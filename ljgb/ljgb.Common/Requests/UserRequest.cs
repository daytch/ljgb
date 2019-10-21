using Microsoft.AspNetCore.Identity;

namespace ljgb.Common.Requests
{
    public class UserRequest
    {
        public IdentityUser user { get; set; }
        public string password { get; set; }
    }
}
