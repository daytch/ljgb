using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface ITransactionLevel
    {
        Task<TransactionLevelResponse> GetAll();

        Task<TransactionLevelResponse> GetCurrentLevel(TransactionLevelRequest request);

        Task<TransactionLevelResponse> GetNextLevel(TransactionLevelRequest request);

      
    }
}
