using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Requests
{
    public class NegoBarangRequest : RequestBase
    {
        public long ID { get; set; }
        public long UserProfileID { get; set; }
        public long BarangID { get; set; }
        public string TypePenawaran { get; set; }
        public long Harga { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
        public List<long> ListWarna { get; set; }
        public string UserName { get; set; }
        public long TypeBarangID { get; set; }
    }
}
