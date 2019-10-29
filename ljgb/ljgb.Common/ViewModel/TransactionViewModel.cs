using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.ViewModel
{
    public class TransactionViewModel
    {
        public long ID { get; set; }
        public UserProfileViewModel Buyer { get; set; }
        public UserProfileViewModel Seller { get; set; }
        public NegoBarangViewModel NegoBarang { get; set; }
        public TransactionLevelViewModel TrasanctionLevel { get; set; }

        public string NamaDealer { get; set; }
        public string NamaBarang { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
    }
}
