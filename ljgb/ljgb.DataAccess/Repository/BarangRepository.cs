using ljgb.Common.ViewModel;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ljgb.Common.Responses;

namespace ljgb.DataAccess.Repository
{
    public class BarangRepository : IBarang
    {
        ljgbContext db;
        public BarangRepository(ljgbContext _db)
        {
            db = _db;
        }
        public Task<long> AddPost(Barang model)
        {
            throw new NotImplementedException();
        }

        public Task<long> DeletePost(long ID)
        {
            throw new NotImplementedException();
        }

        public List<Car> GetLowestAsk(string kota, int total)
        {
            if (db != null)
            {
                try
                {
                    return db.Set<Car>().FromSql("EXEC sp_GetLowestAsk {0}, {1}", kota, total).AsNoTracking().ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }

        public List<Car> GetListNormal(string kota, int total)
        {
            if (db != null)
            {
                try
                {
                    return db.Set<Car>().FromSql("EXEC sp_GetListNormal {0}, {1}", kota, total).AsNoTracking().ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }
        // 

        public List<Car> GetHighestBid(string kota, int total)
        {

            if (db != null)
            {
                try
                {
                    return db.Set<Car>().FromSql("EXEC sp_GetHighestBid {0}, {1}", kota, total).AsNoTracking().ToList();
                }
                catch (Exception ex)
                {

                    throw;
                }

            }

            return null;
        }
        public async Task<List<BarangViewModel>> GetAll()
        {

            if (db != null)
            {
                try
                {
                    return await (from model in db.Barang
                                  where model.RowStatus == true
                                  select new BarangViewModel
                                  {
                                      Id = model.Id,
                                      Name = model.Name,

                                      Created = model.Created,
                                      CreatedBy = model.CreatedBy,
                                      Modified = model.Modified,
                                      ModifiedBy = model.ModifiedBy,
                                      RowStatus = model.RowStatus
                                  }).ToListAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }

            }

            return null;
        }

        public Task<BarangViewModel> GetPost(long ID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePost(Barang model)
        {
            throw new NotImplementedException();
        }
    }
}
