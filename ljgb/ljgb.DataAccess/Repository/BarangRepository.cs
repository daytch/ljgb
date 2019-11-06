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
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BarangRepository(ljgbContext _db)
        {
            db = _db;
        }
        public async Task<long> AddPost(Barang model)
        {
            if (db != null)
            {
                await db.Barang.AddAsync(model);
                await db.SaveChangesAsync();

                return model.Id;
            }

            return 0;
        }

        public async Task<long> DeletePost(long ID)
        {

            int result = 0;

            if (db != null)
            {
                //Find the warna for specific warna id
                Barang barang = await db.Barang.FirstOrDefaultAsync(x => x.Id == ID);

                if (barang != null)
                {
                    barang.RowStatus = false;
                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
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
                    throw;
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

                    throw;
                }

            }

            return null;
        }

        public async Task<BarangResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            BarangResponse response = new BarangResponse();
            try
            {
                if (db !=null)
                {
                    var query = (from brg in db.Barang
                                 join warna in db.Warna
                                 on brg.WarnaId equals warna.Id
                                 join type in db.TypeBarang
                                 on brg.TypeBarangId equals type.Id
                                 join model in db.ModelBarang
                                 on type.ModelBarangId equals model.Id
                                 join merk in db.Merk
                                 on model.MerkId equals merk.Id
                                 where merk.RowStatus == true
                                 && model.RowStatus == true
                                 select new
                                 {
                                     brg.Id,
                                     brg.Name,
                                     brg.HargaOtr,
                                     namaType = type.Name,
                                     namaModelBarang = model.Name,
                                     NamaMerk = merk.Name,
                                     brg.TypeBarangId,
                                     type.ModelBarangId,
                                     model.MerkId,
                                     brg.WarnaId,
                                     namaWarna = warna.Name,
                                     brg.Created,
                                     brg.CreatedBy,
                                     brg.Modified,
                                     brg.ModifiedBy,
                                     brg.RowStatus

                                 }
                           );

                    int totalRecords = query.Count();
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        query = query.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                                            p.namaType.ToString().ToLower().Contains(search.ToLower()) ||
                                            p.namaModelBarang.ToString().ToLower().Contains(search.ToLower()) ||
                                            p.namaWarna.ToString().ToLower().Contains(search.ToLower()) ||
                                            p.NamaMerk.ToLower().Contains(search.ToLower()));
                    }
                    int recFilter = query.Count();
                    response.ListModel = await (from model in query
                                                where model.RowStatus == true
                                                select new BarangViewModel
                                                {
                                                    Id = model.Id,
                                                    Name = model.Name,
                                                    HargaOtr = model.HargaOtr,
                                                    NamaTypeBarang = model.namaType,
                                                    TypeBarangId = model.TypeBarangId,
                                                    NamaModelBarang = model.namaModelBarang,
                                                    ModelBarangID = model.ModelBarangId,
                                                    NamaMerk = model.NamaMerk,
                                                    MerkBarangID = model.MerkId,
                                                    NamaWarna = model.namaWarna,
                                                    WarnaId = model.WarnaId,
                                                    Created = model.Created,
                                                    CreatedBy = model.CreatedBy,
                                                    Modified = model.Modified,
                                                    ModifiedBy = model.ModifiedBy,
                                                    RowStatus = model.RowStatus
                                                }).Skip(startRec).Take(pageSize).ToListAsync();
                    response.draw = Convert.ToInt32(draw);
                    response.recordsTotal = totalRecords;
                    response.recordsFiltered = recFilter;
                    response.Message = "Load Success";
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = "Opps, Something Error with System Righ Now !";
                    response.IsSuccess = false;
                }             

            }
            catch (Exception ex)
            {

                response.Message = ex.ToString();
                response.IsSuccess = false;
            }



            return response;
        }

        public Task<BarangViewModel> GetPost(long ID)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePost(Barang model)
        {
            try
            {
                if (db != null)
                {
                    Barang barang = await db.Barang.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

                    if (barang != null)
                    {
                        barang.Name = model.Name;
                        barang.HargaOtr = model.HargaOtr;
                        barang.WarnaId = model.WarnaId;
                        barang.TypeBarangId = model.TypeBarangId;
                        barang.PhotoPath = model.PhotoPath;
                        barang.Modified = model.Modified;
                        barang.ModifiedBy = model.ModifiedBy;

                        await db.SaveChangesAsync();

                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return true;
        }
    }
}
