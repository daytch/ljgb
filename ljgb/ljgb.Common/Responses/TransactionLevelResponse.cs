using ljgb.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class TransactionLevelResponse : ResponseBase
    {
        public List<TransactionLevelViewModel> ListTransactionLevel { get; set; }

        public TransactionLevelViewModel model { get; set; }
        public TransactionLevelResponse()
        {
            ListTransactionLevel = new List<TransactionLevelViewModel>();
        }
    }
}
