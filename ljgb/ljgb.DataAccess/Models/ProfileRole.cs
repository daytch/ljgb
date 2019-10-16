using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Models
{
    public partial class ProfileRole
    {
        public long Id { get; set; }
        public long UserProfileId { get; set; }
        public int RoleId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public Role Role { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
