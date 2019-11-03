using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class TransactionJournal
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public long BuyerId { get; set; }
        public long SellerId { get; set; }
        public long NegoBarangId { get; set; }
        public int TransactionLevelId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
