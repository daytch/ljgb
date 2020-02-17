using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class MerkRank
    {
        public int Id { get; set; }
        public long MerkId { get; set; }
        public int Rank { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
    }
}
