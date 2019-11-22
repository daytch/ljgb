using System;

namespace ljgb.Common.ViewModel
{
    public class UserProfileViewModel
    {
        public long ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Telp { get; set; }
        public string Facebook { get; set; }
        public string IG { get; set; }
        public string JenisKelamin { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
        public string Alamat { get; set; }
        public string Photopath { get; set; }
    }
}
