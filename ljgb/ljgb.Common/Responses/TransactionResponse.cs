using ljgb.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class TransactionResponse : ResponseBase
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<TransactionViewModel> ListTransaction { get; set; }

        public List<TransactionJournalViewModel> ListJournalByTransaction { get; set; }
        public List<TransactionHistoryViewModel> ListHistory { get; set; }

        public TransactionResponse()
        {
            ListTransaction = new List<TransactionViewModel>();
        }

    }

   
}
