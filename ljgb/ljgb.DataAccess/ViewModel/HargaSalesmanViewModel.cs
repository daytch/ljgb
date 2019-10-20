using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.DataAccess.ViewModel
{
    public class HargaSalesmanViewModel
    {
        public long ID { get; set; }
        public long Discount { get; set; }
        public long BarangID { get; set; }
        public long UserProfileID { get; set; }
        public long HargaFinal { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
    }
}
