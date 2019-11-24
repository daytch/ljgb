using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using ljgb.Common.Responses;
using ljgb.Common.ViewModel;

namespace ljgb.DataAccess.Repository
{
    public class DealerRepository : IDealer
    {
        ljgbContext db;
        public DealerRepository(ljgbContext _db)
        {
            db = _db;
        }

        public async Task<long> AddPost(Dealer Request)
        {
            long result = 0;
            try
            {
                await db.Dealer.AddAsync(Request);
                result = db.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        public async Task<long> DeletePost(long ID, string username)
        {
            long result = 0;
            if (db != null)
            {
                try
                {

                    var dealer = await db.Dealer.FirstOrDefaultAsync(x => x.Id == ID);

                    if (dealer != null)
                    {
                        dealer.Modified = DateTime.Now;
                        dealer.ModifiedBy = username;
                        dealer.RowStatus = false;
                        db.Dealer.Update(dealer);

                        result = await db.SaveChangesAsync();
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return result;
        }

        public async Task<DealerResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            DealerResponse response = new DealerResponse();
            if (db != null)
            {
                try
                {

                    var query = (from dealer in db.Dealer
                                 join kota in db.Kota
                                 on dealer.KotaId equals kota.Id
                                 where dealer.RowStatus == true && kota.RowStatus == true
                                 select new
                                 {
                                     dealer.Id,
                                     dealer.Name,
                                     dealer.Kode,
                                     dealer.Alamat,
                                     KotaID = kota.Id,
                                     KotaName = kota.Name,
                                     dealer.Telepon,
                                     dealer.PejabatDealer,
                                 });

                    int totalRecords = query.Count();
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        query = query.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                                            p.Kode.ToString().ToLower().Contains(search.ToLower()) ||
                                            p.Alamat.ToString().ToLower().Contains(search.ToLower()) ||
                                            p.KotaName.ToString().ToLower().Contains(search.ToLower()) ||
                                            p.PejabatDealer.ToLower().Contains(search.ToLower()));
                    }

                    int recFilter = query.Count();
                    response.ListModel = await (from model in query
                                                select new DealerViewModel
                                                {
                                                    ID = model.Id,
                                                    Name = model.Name,
                                                    Kode = model.Kode,
                                                    Alamat = model.Alamat,
                                                    KotaID = model.KotaID,
                                                    KotaName = model.KotaName,
                                                    Telepon = model.Telepon,
                                                    PejabatDealer = model.PejabatDealer
                                                }).Skip(startRec).Take(pageSize).ToListAsync();
                    response.draw = Convert.ToInt32(draw);
                    response.recordsTotal = totalRecords;
                    response.recordsFiltered = recFilter;
                    response.Message = "Load Success";
                    response.IsSuccess = true;
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }
            }
            return response;
        }

        public async Task<List<Dealer>> GetDealerByKotaID(int KotaID)
        {
            if (db != null)
            {
                return await db.Dealer.Where(x => x.RowStatus == true && x.KotaId == KotaID).Select(x => new Dealer() { Id = x.Id, Kode = x.Kode, Name = x.Name }).ToListAsync();
            }

            return null;
        }

        public async Task<Dealer> GetPost(long ID)
        {
            if (db != null)
            {
                try
                {
                    return await (from dealer in db.Dealer
                                  where dealer.RowStatus == true && dealer.Id == ID
                                  select dealer
                                ).FirstOrDefaultAsync();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            return null;
        }

        public async Task<bool> UpdatePost(Dealer request)
        {
            if (db != null)
            {
                try
                {
                    db.Dealer.Update(request);
                    await db.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return false;
        }
    }
}
