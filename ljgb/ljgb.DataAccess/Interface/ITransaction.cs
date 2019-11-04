using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Model;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface ITransaction
    {
        Task<TransactionResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw);

        Task<TransactionResponse> GetPost(TransactionRequest model);

        Task<TransactionResponse> AddPost(TransactionRequest model);

        Task<long> SaveTransaction(Transaction model);

        Task<TransactionResponse> DeletePost(TransactionRequest model);

        Task<TransactionResponse> UpdatePost(TransactionRequest model);
        Task<TransactionResponse> CancelTransaction(TransactionRequest req);
        Task<TransactionResponse> ApproveTransaction(TransactionRequest req);

        Task<TransactionResponse> GetJournalByTransaction(TransactionRequest req);
    }
}
