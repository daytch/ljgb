using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class RegisterResponse
    {
        public bool Succeeded { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}
