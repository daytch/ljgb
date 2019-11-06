using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class Provinsi
    {
        public Provinsi()
        {
            Kota = new HashSet<Kota>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public virtual ICollection<Kota> Kota { get; set; }
    }
}
