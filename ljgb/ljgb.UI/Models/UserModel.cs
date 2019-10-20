using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ljgb.UI.Models
{
    public class UserModel
    {
        public IdentityUser user { get; set; }
        public string password { get; set; }
    }
}
