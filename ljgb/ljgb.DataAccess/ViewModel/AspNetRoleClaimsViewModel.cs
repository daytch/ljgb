using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.DataAccess.ViewModel
{
    public class AspNetRoleClaimsViewModel
    {
        public int ID { get; set; }
        public string RoleID { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
