using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
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

        public virtual Role Role { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
