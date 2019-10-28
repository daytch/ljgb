using System;

namespace ljgb.Common.ViewModel
{
    public class BuyerViewModel
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
