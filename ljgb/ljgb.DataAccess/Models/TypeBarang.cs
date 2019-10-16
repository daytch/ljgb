using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Models
{
    public partial class TypeBarang
    {
        public TypeBarang()
        {
            Barang = new HashSet<Barang>();
        }

        public long Id { get; set; }
        public long ModelBarangId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public ModelBarang ModelBarang { get; set; }
        public ICollection<Barang> Barang { get; set; }
    }
}
