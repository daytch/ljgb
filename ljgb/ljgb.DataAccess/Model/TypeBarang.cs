using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
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

        public virtual ModelBarang ModelBarang { get; set; }
        public virtual ICollection<Barang> Barang { get; set; }
    }
}
