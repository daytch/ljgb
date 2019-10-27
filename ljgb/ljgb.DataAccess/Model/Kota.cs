using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class Kota
    {
        public long Id { get; set; }
        public long ProvinsiId { get; set; }
        public string Nama { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public virtual Provinsi Provinsi { get; set; }
    }
}
