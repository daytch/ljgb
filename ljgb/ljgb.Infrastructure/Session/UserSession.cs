using System;
using System.Collections.Generic;

namespace ljgb.Infrastructure.Session
{
    [Serializable]
    public class UserSession
    {
        public string Token { get; set; }
        public string AppUserId { get; set; }
        public string AppUsername { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public bool SysAdmin { get; set; }

        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationDomain { get; set; }

        public bool? Gender { get; set; }


        public List<string> Teams { get; set; }

        public string Fullname
        {
            get
            {
                if (string.IsNullOrEmpty(Lastname))
                    return String.Format("{0}", Firstname).Trim();
                else
                    return String.Format("{1},{0}", Firstname, Lastname.ToUpper()).Trim();
            }
        }
        public string[] Roles { get; set; }

    }

}
