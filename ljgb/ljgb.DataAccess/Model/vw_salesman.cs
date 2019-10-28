using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class vw_salesman
    {
        public Int64 ID { get; set; }
        public int DetailID { get; set; }
        public string Nama { get; set; }
        public string Email { get; set; }
        public string Telp { get; set; }
        public Nullable<DateTime> VerifiedDate { get; set; }
        public string VerifiedBy { get; set; }
    }
}
