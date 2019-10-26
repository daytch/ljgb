using ljgb.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class TransactionResponse : ResponseBase
    {
        public List<TransactionViewModel> ListTransaction { get; set; }
        public TransactionResponse()
        {
            ListTransaction = new List<TransactionViewModel>();
        }
    }
}
