using Microsoft.AspNetCore.Identity;
using System;

namespace ljgb.Common.Requests
{
    public class UserRequest : BaseRequest
    {
        public IdentityUser user { get; set; }
        public string password { get; set; }

        public string HTMLTag { get; set; }
        public string EmailSubject { get; set; }
        public bool RememberMe { get; set; }
        public bool lockoutOnFailure { get; set; }

        public int DetailID { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime VerifiedDate { get; set; }
    }
}
