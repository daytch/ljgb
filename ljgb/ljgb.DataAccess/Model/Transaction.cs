using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class Transaction
    {
        public long Id { get; set; }
        public long BuyerId { get; set; }
        public long SellerId { get; set; }
        public long NegoBarangId { get; set; }
        public int TransactionLevelId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public virtual UserProfile Buyer { get; set; }
        public virtual NegoBarang NegoBarang { get; set; }
        public virtual UserProfile Seller { get; set; }
        public virtual TransactionLevel TransactionLevel { get; set; }
    }
}
