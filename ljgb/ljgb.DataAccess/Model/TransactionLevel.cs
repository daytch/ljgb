using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class TransactionLevel
    {
        public TransactionLevel()
        {
            Transaction = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public int TransactionStatusId { get; set; }
        public int TransactionStepId { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public virtual TransactionStatus TransactionStatus { get; set; }
        public virtual TransactionStep TransactionStep { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
