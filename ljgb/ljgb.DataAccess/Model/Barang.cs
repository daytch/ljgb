using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class Barang
    {
        public Barang()
        {
            NegoBarang = new HashSet<NegoBarang>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long HargaOtr { get; set; }
        public long WarnaId { get; set; }
        public long TypeBarangId { get; set; }
        public string PhotoPath { get; set; }
        public long? JumlahKlik { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public virtual TypeBarang TypeBarang { get; set; }
        public virtual Warna Warna { get; set; }
        public virtual ICollection<NegoBarang> NegoBarang { get; set; }
    }
}
