using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public DateTime Expired { get; set; }
        public bool Succeeded { get; set; }
    }
}
