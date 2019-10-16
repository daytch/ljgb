using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Models
{
    public partial class Barang
    {
        public Barang()
        {
            HargaSalesman = new HashSet<HargaSalesman>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long HargaOtr { get; set; }
        public long WarnaId { get; set; }
        public long TypeBarangId { get; set; }
        public string PhotoPath { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public TypeBarang TypeBarang { get; set; }
        public Warna Warna { get; set; }
        public ICollection<HargaSalesman> HargaSalesman { get; set; }
    }
}
