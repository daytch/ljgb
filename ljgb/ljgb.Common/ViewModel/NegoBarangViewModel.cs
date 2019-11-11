using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.ViewModel
{
    public class NegoBarangViewModel
    {
        public long ID { get; set; }
        public long UserProfileID { get; set; }
        public UserProfileViewModel userProfieViewModel { get; set; }
        public long BarangID { get; set; }
        public BarangViewModel Barang { get; set; }
        public string TypePenawaran { get; set; }
        public long Harga { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
        public string NamaBarang { get; set; }
        public string Warna { get; set; }
        public long HargaOTR { get; set; }
        public long MerkID { get; set; }
        public long ModelBarangID { get; set; }
        public long TypeBarangID { get; set; }
        public long WarnaID { get; set; }
    }
}
