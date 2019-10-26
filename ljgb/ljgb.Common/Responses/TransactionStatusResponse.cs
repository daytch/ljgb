using ljgb.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class TransactionStatusResponse
    {
        public List<TransactionStatusViewModel> ListTransactionStatus { get; set; }

        public TransactionStatusResponse()
        {
            ListTransactionStatus = new List<TransactionStatusViewModel>();
        }
    }
}
