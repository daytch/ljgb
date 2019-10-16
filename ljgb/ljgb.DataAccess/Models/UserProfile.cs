using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Models
{
    public partial class UserProfile
    {
        public UserProfile()
        {
            ProfileRole = new HashSet<ProfileRole>();
        }

        public long Id { get; set; }
        public string Nama { get; set; }
        public string Email { get; set; }
        public string Telp { get; set; }
        public string Facebook { get; set; }
        public string Ig { get; set; }
        public string JenisKelamin { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public ICollection<ProfileRole> ProfileRole { get; set; }
    }
}
