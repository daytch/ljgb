using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.Common.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ljgb.DataAccess.Repository
{
    public class DealerRepository : IDealer
    {
        ljgbContext db;
        public DealerRepository(ljgbContext _db)
        {
            db = _db;
        }

        public async Task<List<Dealer>> GetDealerByKotaID(int KotaID)
        {
            if (db != null)
            {
                return await db.Dealer.Where(x => x.RowStatus == true && x.KotaId == KotaID).Select(x => new Dealer() { Id = x.Id, Kode = x.Kode, Name = x.Name }).ToListAsync();
            }

            return null;
        }

    }
}
