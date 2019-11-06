using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class Merk
    {
        public Merk()
        {
            ModelBarang = new HashSet<ModelBarang>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public virtual ICollection<ModelBarang> ModelBarang { get; set; }
    }
}
