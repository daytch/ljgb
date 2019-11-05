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
using ljgb.Common.Requests;
using System.Data.SqlClient;

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

        public List<Car> GetRelatedProducts(int id)
        {
            if (db != null)
            {
                try
                {
                    return db.Set<Car>().FromSql("EXEC sp_GetRelatedProductByID {0}", id).AsNoTracking().ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }

        public async Task<Position> GetAskPosition(int id, int nominal)
        {
            if (db != null)
            {
                try
                {
                    return await db.Set<Position>().FromSql("EXEC sp_GetAskPosition {0}, {1}", id, nominal).FirstOrDefaultAsync();//.AsNoTracking().ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }

        public async Task<Position> GetBidPosition(int id, int nominal)
        {
            if (db != null)
            {
                try
                {
                    return await db.Set<Position>().FromSql("EXEC sp_GetBidPosition {0}, {1}", id, nominal).FirstOrDefaultAsync();//.AsNoTracking().ToList();
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
                    throw ex;
                }

            }

            return null;
        }

        public List<CarAsks> GetAllAsksById(BarangRequest req)
        {
            if (db != null)
            {
                try
                {
                    int result = 0;
                    return db.Set<CarAsks>().FromSql("EXEC sp_GetAllbIDSByID_With_Paging {0},{1},{2},{3},{4},{5},{6}",
                        req.ID, req.start, req.limit, null, null, req.max, result).AsNoTracking().ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return null;
        }

        public async Task<CarDetail> GetBarangDetail(int id)
        {

            if (db != null)
            {
                try
                {
                    return await db.Set<CarDetail>().FromSql("EXEC sp_GetDetailBareng {0}", id).AsNoTracking().FirstAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
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
                    throw ex;
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
