using ljgb.Common.Requests;
using ljgb.Common.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface ITransaction
    {
        Task<TransactionResponse> GetAll();

        Task<TransactionResponse> GetPost(TransactionRequest model);

        Task<TransactionResponse> AddPost(TransactionRequest model);

        Task<TransactionResponse> DeletePost(TransactionRequest model);

        Task<TransactionResponse> UpdatePost(TransactionRequest model);
        Task<TransactionResponse> CancelTransaction(TransactionRequest req);
        Task<TransactionResponse> ApproveTransaction(TransactionRequest req);
    }
}
