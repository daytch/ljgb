using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class Barang
    {
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
        public long KotaId { get; set; }
        public int? Year { get; set; }

        public virtual Kota Kota { get; set; }
        public virtual TypeBarang TypeBarang { get; set; }
        public virtual Warna Warna { get; set; }
    }
}
