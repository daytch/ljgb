using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class HargaSalesman
    {
        public long Id { get; set; }
        public long BarangId { get; set; }
        public long Discount { get; set; }
        public long UserProfileId { get; set; }
        public long HargaFinal { get; set; }
        public DateTime Created { get; set; }
        public string Createdby { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public virtual Barang Barang { get; set; }
    }
}
