using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class NegoBarang
    {
        public long Id { get; set; }
        public long UserProfileId { get; set; }
        public long BarangId { get; set; }
        public string TypePenawaran { get; set; }
        public long Harga { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
        public bool? HasTransaction { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
