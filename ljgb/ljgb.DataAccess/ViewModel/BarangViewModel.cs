using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.DataAccess.ViewModel
{
    public class BarangViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public long HargaOTR { get; set; }
        public long WarnaID { get; set; }
        public long TypeBarangID { get; set; }
        public string PhotoPath { get; set; }
        public DateTime Created { get; set; }
        public string CreadtedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
    }
}
