using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.ViewModel
{
    public class TransactionJournalViewModel
    {
        public long ID { get; set; }
        public long BuyerID { get; set; }
        public long SellerID { get; set; }
        public long NegoBarangID { get; set; }
        public int TransactionLevelID { get; set; }

        public string TransactionStepName { get; set; }
        public string TransactionStatusName { get; set; }
        public string BuyerName { get; set; }
        public string SellerName { get; set; }

        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
    }
}
