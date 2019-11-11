using ljgb.Common.Responses;
using ljgb.DataAccess.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
   public interface IDealer
    {
        Task<long> AddPost(Dealer Request);

        Task<long> DeletePost(long ID);

        Task<List<Dealer>> GetDealerByKotaID(int KotaID);

        Task<DealerResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw);

        Task<Dealer> GetPost(long ID);

        Task<bool> UpdatePost(Dealer request);

       
    }
}
