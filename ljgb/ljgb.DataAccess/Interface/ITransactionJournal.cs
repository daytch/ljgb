using ljgb.Common.Responses;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface ITransactionJournal
    {

        Task<long> SaveTransactionJournal(TransactionJournal model);
        Task<TransactionResponse> GetHistory(long ID);
    }
}
