using ljgb.DataAccess.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
   public interface IDealer
    {
        Task<List<Dealer>> GetDealerByKotaID(int KotaID);
    }
}
