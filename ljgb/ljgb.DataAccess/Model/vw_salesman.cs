using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class vw_salesman
    {
        public long ID { get; set; }
        public long DetailID { get; set; }
        public string Nama { get; set; }
        public string Email { get; set; }
        public string Telp { get; set; }
        public DateTime VerifiedDate { get; set; }
        public string VerifiedBy { get; set; }
    }
}
