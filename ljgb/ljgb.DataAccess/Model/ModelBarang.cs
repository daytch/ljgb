using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class ModelBarang
    {
        public ModelBarang()
        {
            TypeBarang = new HashSet<TypeBarang>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long MerkId { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string Createdby { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
        public string Category { get; set; }

        public virtual Merk Merk { get; set; }
        public virtual ICollection<TypeBarang> TypeBarang { get; set; }
    }
}
