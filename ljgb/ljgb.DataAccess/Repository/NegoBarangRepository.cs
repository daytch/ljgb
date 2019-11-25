using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using ljgb.Common.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace ljgb.DataAccess.Repository
{
    public class NegoBarangRepository : INegoBarang
    {
        ljgbContext db;
        public NegoBarangRepository(ljgbContext _db)
        {
            db = _db;
        }
        public async Task<NegoBarangResponse> AddPost(NegoBarangRequest model)
        {
            NegoBarangResponse response = new NegoBarangResponse();


            if (db != null)
            {
                try
                {

                    NegoBarang negoBarang = new NegoBarang();

                    negoBarang.UserProfileId = db.UserProfile.Where(x => x.RowStatus == true && x.Email == model.UserName).Select(x=>x.Id).FirstOrDefaultAsync().Result;
                    negoBarang.BarangId = model.BarangID;
                    negoBarang.TypePenawaran = model.TypePenawaran;
                    negoBarang.Harga = model.Harga;
                    negoBarang.Created = DateTime.Now;
                    negoBarang.CreatedBy = model.UserName;
                    negoBarang.RowStatus = true;

                    await db.NegoBarang.AddAsync(negoBarang);
                    await db.SaveChangesAsync();


                }
                catch (Exception ex)
                {

                    response.IsSuccess = false;
                    response.Message = ex.ToString();
                }
            }


            return response;
        }

        public async Task<long> AddPost(NegoBarang model)
        {
            long response = 0;


            if (db != null)
            {
                try
                {

                    //NegoBarang negoBarang = new NegoBarang();

                    //negoBarang.UserProfileId = model.UserProfileId;
                    //negoBarang.BarangId = model.BarangId;
                    //negoBarang.TypePenawaran = model.TypePenawaran;
                    //negoBarang.Harga = model.Harga;
                    //negoBarang.Created = DateTime.Now;
                    //negoBarang.CreatedBy = model.CreatedBy;
                    //negoBarang.RowStatus = true;
                   
                    await db.NegoBarang.AddAsync(model);
                    response = await db.SaveChangesAsync();


                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }


            return response;
        }

        public async Task<NegoBarang> GetNegoBarang(NegoBarang model)
        {
            NegoBarang response = new NegoBarang();

            if (db != null)
            {
                try
                {
                    response = await db.NegoBarang.Where(x => x.RowStatus == true
                    && x.BarangId == model.BarangId && x.Harga == model.Harga && x.TypePenawaran == model.TypePenawaran).FirstOrDefaultAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return response;
        }

        public async Task<NegoBarangResponse> DeletePost(NegoBarangRequest model)
        {
            NegoBarangResponse response = new NegoBarangResponse();

            if (db != null)
            {
                try
                {
                    NegoBarang negoBarang = await db.NegoBarang.Where(x => x.RowStatus == true && x.Id == model.ID).FirstOrDefaultAsync();
                    if (negoBarang != null)
                    {
                        negoBarang.RowStatus = false;
                        await db.SaveChangesAsync();

                        response.Message = "Data has been Saved";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Data Not Found";
                    }
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }

            }
            return response;
        }

        public async Task<bool> DeletePost(long ID)
        {
            bool result = false;

            if (db != null)
            {
                try
                {
                    NegoBarang negoBarang = await db.NegoBarang.Where(x => x.RowStatus == true && x.Id == ID).FirstOrDefaultAsync();
                    if (negoBarang != null)
                    {
                        negoBarang.RowStatus = false;
                        await db.SaveChangesAsync();

                        result = true;
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            return result;
        }

        public async Task<NegoBarangResponse> GetAll()
        {
            NegoBarangResponse response = new NegoBarangResponse();
            if (db != null)
            {
                try
                {
                    response.ListModel = await (from model in db.NegoBarang
                                                where model.RowStatus == true
                                                select new NegoBarangViewModel
                                                {
                                                    ID = model.Id,
                                                    UserProfileID = model.UserProfileId,
                                                    BarangID = model.BarangId,
                                                    TypePenawaran = model.TypePenawaran,
                                                    Harga = model.Harga,
                                                    Created = model.Created,
                                                    CreatedBy = model.CreatedBy,
                                                    Modified = model.Modified,
                                                    ModifiedBy = model.ModifiedBy,
                                                    RowStatus = model.RowStatus
                                                }).ToListAsync();
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                }

            }

            return response;
        }

        public async Task<NegoBarangResponse> GetPost(NegoBarangRequest req)
        {
            NegoBarangResponse response = new NegoBarangResponse();
            if (db != null)
            {
                try
                {
                    response.ListModel = await (from model in db.NegoBarang
                                                where model.RowStatus == true && model.Id == req.ID
                                                select new NegoBarangViewModel
                                                {
                                                    ID = model.Id,
                                                    UserProfileID = model.UserProfileId,
                                                    BarangID = model.BarangId,
                                                    TypePenawaran = model.TypePenawaran,
                                                    Harga = model.Harga,
                                                    Created = model.Created,
                                                    CreatedBy = model.CreatedBy,
                                                    Modified = model.Modified,
                                                    ModifiedBy = model.ModifiedBy,
                                                    RowStatus = model.RowStatus
                                                }).ToListAsync();
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                }

            }

            return response;
        }

        public async Task<NegoBarangResponse> UpdatePost(NegoBarangRequest model)
        {
            NegoBarangResponse response = new NegoBarangResponse();

            if (db != null)
            {
                try
                {
                    NegoBarang negoBarang = await db.NegoBarang.Where(x => x.RowStatus == true && x.Id == model.ID).FirstOrDefaultAsync();
                    if (negoBarang != null)
                    {
                        negoBarang.UserProfileId = model.UserProfileID;
                        negoBarang.BarangId = model.BarangID;
                        negoBarang.TypePenawaran = model.TypePenawaran;
                        negoBarang.Harga = model.Harga;
                        negoBarang.Modified = DateTime.Now;
                        negoBarang.ModifiedBy = model.ModifiedBy;
                        await db.SaveChangesAsync();

                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Data Not Found";
                    }
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }

            }
            return response;
        }

        public async Task<NegoBarangResponse> GetAllASK(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {

            NegoBarangResponse response = new NegoBarangResponse();

            if (db != null)
            {
                try
                {
                    var query = (from negoBarang in db.NegoBarang
                                 join barang in db.Barang
                                 on negoBarang.BarangId equals barang.Id
                                 join warna in db.Warna
                                 on barang.WarnaId equals warna.Id
                                 join tBarang in db.TypeBarang
                                 on barang.TypeBarangId equals tBarang.Id
                                 join mdlBarang in db.ModelBarang
                                 on tBarang.ModelBarangId equals mdlBarang.Id
                                 join merkBarang in db.Merk
                                 on mdlBarang.MerkId equals merkBarang.Id
                                 where negoBarang.RowStatus == true &&
                                 barang.RowStatus == true &&
                                 warna.RowStatus == true &&
                                 tBarang.RowStatus == true &&
                                 mdlBarang.RowStatus == true &&
                                 merkBarang.RowStatus == true &&
                                 negoBarang.TypePenawaran == "ASK"
                                 select new
                                 {
                                     negoBarang.Id,
                                     NamaBarang = (merkBarang.Name + " " + mdlBarang.Name + " " + tBarang.Name + " " + warna.Name),
                                     negoBarang.Created,
                                     negoBarang.CreatedBy,
                                     negoBarang.Modified,
                                     negoBarang.ModifiedBy,
                                     negoBarang.RowStatus,
                                     barang.HargaOtr,
                                     negoBarang.Harga,
                                     MerkID = merkBarang.Id,
                                     ModelBarangID = mdlBarang.Id,
                                     TypeBarangID = tBarang.Id,
                                     WarnaID = warna.Id,
                                     barangID = barang.Id

                                 });
                    int totalRecords = query.Count();
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        query = query.Where(p => p.NamaBarang.ToString().ToLower().Contains(search.ToLower()) ||
                                    p.HargaOtr.ToString().ToLower().Contains(search.ToLower()) ||
                                    p.Harga.ToString().ToLower().Contains(search.ToLower()));


                    }

                    //response = SortByColumnWithOrder(order, orderDir, response);

                    int recFilter = query.Count();

                    response.ListModel = await (from q in query
                                                select new NegoBarangViewModel
                                                {
                                                    ID = q.Id,
                                                    Harga = q.Harga,
                                                    HargaOTR = q.HargaOtr,
                                                    NamaBarang = q.NamaBarang,
                                                    MerkID = q.MerkID,
                                                    ModelBarangID = q.ModelBarangID,
                                                    TypeBarangID = q.TypeBarangID,
                                                    WarnaID = q.WarnaID,
                                                    BarangID = q.barangID

                                                }

                                        ).Skip(startRec).Take(pageSize).ToListAsync();

                    response.draw = Convert.ToInt32(draw);
                    response.recordsTotal = totalRecords;
                    response.recordsFiltered = recFilter;
                    response.Message = "Success";
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }

            }
            return response;
        }

        public async Task<long> UpdatePost(NegoBarang model)
        {
            long result = 0;

            if (db != null)
            {
                try
                {
                    NegoBarang negoBarang = await db.NegoBarang.Where(x => x.RowStatus == true && x.Id == model.Id).FirstOrDefaultAsync();
                    if (negoBarang != null)
                    {
                        negoBarang.UserProfileId = model.UserProfileId;
                        negoBarang.BarangId = model.BarangId;
                        negoBarang.TypePenawaran = model.TypePenawaran;
                        negoBarang.Harga = model.Harga;
                        negoBarang.Modified = DateTime.Now;
                        negoBarang.ModifiedBy = model.ModifiedBy;
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

        public async Task<NegoBarangResponse> GetAllBID(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            NegoBarangResponse response = new NegoBarangResponse();
            if (db != null)
            {
                try
                {
                    var query = (from negoBarang in db.NegoBarang
                                 join barang in db.Barang
                                 on negoBarang.BarangId equals barang.Id
                                 join warna in db.Warna
                                 on barang.WarnaId equals warna.Id
                                 join tBarang in db.TypeBarang
                                 on barang.TypeBarangId equals tBarang.Id
                                 join mdlBarang in db.ModelBarang
                                 on tBarang.ModelBarangId equals mdlBarang.Id
                                 join merkBarang in db.Merk
                                 on mdlBarang.MerkId equals merkBarang.Id
                                 where negoBarang.RowStatus == true &&
                                 barang.RowStatus == true &&
                                 warna.RowStatus == true &&
                                 tBarang.RowStatus == true &&
                                 mdlBarang.RowStatus == true &&
                                 merkBarang.RowStatus == true &&
                                 negoBarang.TypePenawaran.ToLower() == "BID"
                                 select new
                                 {
                                     negoBarang.Id,
                                     NamaBarang = (merkBarang.Name + " " + mdlBarang.Name + " " + tBarang.Name + " " + warna.Name),
                                     negoBarang.Created,
                                     negoBarang.CreatedBy,
                                     negoBarang.Modified,
                                     negoBarang.ModifiedBy,
                                     negoBarang.RowStatus,
                                     barang.HargaOtr,
                                     negoBarang.Harga,
                                     MerkID = merkBarang.Id,
                                     ModelBarangID = mdlBarang.Id,
                                     TypeBarangID = tBarang.Id,
                                     WarnaID = warna.Id,
                                     barangID = barang.Id,
                                     negoBarang.UserProfileId,

                                 });
                    int totalRecords = query.Count();
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        query = query.Where(p => p.NamaBarang.ToString().ToLower().Contains(search.ToLower()) ||
                                    p.HargaOtr.ToString().ToLower().Contains(search.ToLower()) ||
                                    p.Harga.ToString().ToLower().Contains(search.ToLower()));


                    }

                    //response = SortByColumnWithOrder(order, orderDir, response);

                    int recFilter = query.Count();

                    response.ListModel = await (from q in query
                                                select new NegoBarangViewModel
                                                {
                                                    ID = q.Id,
                                                    Harga = q.Harga,
                                                    HargaOTR = q.HargaOtr,
                                                    NamaBarang = q.NamaBarang,
                                                    MerkID = q.MerkID,
                                                    ModelBarangID = q.ModelBarangID,
                                                    TypeBarangID = q.TypeBarangID,
                                                    WarnaID = q.WarnaID,
                                                    BarangID = q.barangID,
                                                    UserProfileID = q.UserProfileId

                                                }

                                        ).Skip(startRec).Take(pageSize).ToListAsync();

                    response.draw = Convert.ToInt32(draw);
                    response.recordsTotal = totalRecords;
                    response.recordsFiltered = recFilter;
                    response.Message = "Success";
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }

            }
            return response;
        }

        public async Task<NegoBarangResponse> GetAllASK(string search, string order, string orderDir, int startRec, int pageSize, int draw, long ID)
        {
            NegoBarangResponse response = new NegoBarangResponse();

            if (db != null)
            {
                try
                {
                    var query = (from negoBarang in db.NegoBarang
                                 join barang in db.Barang
                                 on negoBarang.BarangId equals barang.Id
                                 join warna in db.Warna
                                 on barang.WarnaId equals warna.Id
                                 join tBarang in db.TypeBarang
                                 on barang.TypeBarangId equals tBarang.Id
                                 join mdlBarang in db.ModelBarang
                                 on tBarang.ModelBarangId equals mdlBarang.Id
                                 join merkBarang in db.Merk
                                 on mdlBarang.MerkId equals merkBarang.Id
                                 where negoBarang.RowStatus == true &&
                                 barang.RowStatus == true &&
                                 warna.RowStatus == true &&
                                 tBarang.RowStatus == true &&
                                 mdlBarang.RowStatus == true &&
                                 merkBarang.RowStatus == true &&
                                 negoBarang.TypePenawaran == "ASK" &&
                                 negoBarang.UserProfileId == ID
                                 select new
                                 {
                                     negoBarang.Id,
                                     NamaBarang = (merkBarang.Name + " " + mdlBarang.Name + " " + tBarang.Name + " " + warna.Name),
                                     negoBarang.Created,
                                     negoBarang.CreatedBy,
                                     negoBarang.Modified,
                                     negoBarang.ModifiedBy,
                                     negoBarang.RowStatus,
                                     barang.HargaOtr,
                                     negoBarang.Harga,
                                     MerkID = merkBarang.Id,
                                     ModelBarangID = mdlBarang.Id,
                                     TypeBarangID = tBarang.Id,
                                     WarnaID = warna.Id,
                                     barangID = barang.Id

                                 });
                    int totalRecords = query.Count();
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        query = query.Where(p => p.NamaBarang.ToString().ToLower().Contains(search.ToLower()) ||
                                    p.HargaOtr.ToString().ToLower().Contains(search.ToLower()) ||
                                    p.Harga.ToString().ToLower().Contains(search.ToLower()));


                    }

                    //response = SortByColumnWithOrder(order, orderDir, response);

                    int recFilter = query.Count();

                    response.ListModel = await(from q in query
                                               select new NegoBarangViewModel
                                               {
                                                   ID = q.Id,
                                                   Harga = q.Harga,
                                                   HargaOTR = q.HargaOtr,
                                                   NamaBarang = q.NamaBarang,
                                                   MerkID = q.MerkID,
                                                   ModelBarangID = q.ModelBarangID,
                                                   TypeBarangID = q.TypeBarangID,
                                                   WarnaID = q.WarnaID,
                                                   BarangID = q.barangID

                                               }

                                        ).Skip(startRec).Take(pageSize).ToListAsync();

                    response.draw = Convert.ToInt32(draw);
                    response.recordsTotal = totalRecords;
                    response.recordsFiltered = recFilter;
                    response.Message = "Success";
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }

            }
            return response;
        }

        public async Task<NegoBarangResponse> GetAllBID(string search, string order, string orderDir, int startRec, int pageSize, int draw, long KotaID)
        {
            NegoBarangResponse response = new NegoBarangResponse();
            if (db != null)
            {
                try
                {
                    var query = (from negoBarang in db.NegoBarang
                                 join barang in db.Barang
                                 on negoBarang.BarangId equals barang.Id
                                 join warna in db.Warna
                                 on barang.WarnaId equals warna.Id
                                 join tBarang in db.TypeBarang
                                 on barang.TypeBarangId equals tBarang.Id
                                 join mdlBarang in db.ModelBarang
                                 on tBarang.ModelBarangId equals mdlBarang.Id
                                 join merkBarang in db.Merk
                                 on mdlBarang.MerkId equals merkBarang.Id
                                 where negoBarang.RowStatus == true &&
                                 barang.RowStatus == true &&
                                 warna.RowStatus == true &&
                                 tBarang.RowStatus == true &&
                                 mdlBarang.RowStatus == true &&
                                 merkBarang.RowStatus == true &&
                                 negoBarang.TypePenawaran.ToLower() == "BID" &&
                                 barang.KotaId == KotaID
                                 select new
                                 {
                                     negoBarang.Id,
                                     NamaBarang = (merkBarang.Name + " " + mdlBarang.Name + " " + tBarang.Name + " " + warna.Name),
                                     negoBarang.Created,
                                     negoBarang.CreatedBy,
                                     negoBarang.Modified,
                                     negoBarang.ModifiedBy,
                                     negoBarang.RowStatus,
                                     barang.HargaOtr,
                                     negoBarang.Harga,
                                     MerkID = merkBarang.Id,
                                     ModelBarangID = mdlBarang.Id,
                                     TypeBarangID = tBarang.Id,
                                     WarnaID = warna.Id,
                                     barangID = barang.Id,
                                     negoBarang.UserProfileId,

                                 });
                    int totalRecords = query.Count();
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        query = query.Where(p => p.NamaBarang.ToString().ToLower().Contains(search.ToLower()) ||
                                    p.HargaOtr.ToString().ToLower().Contains(search.ToLower()) ||
                                    p.Harga.ToString().ToLower().Contains(search.ToLower()));


                    }

                    //response = SortByColumnWithOrder(order, orderDir, response);

                    int recFilter = query.Count();

                    response.ListModel = await(from q in query
                                               select new NegoBarangViewModel
                                               {
                                                   ID = q.Id,
                                                   Harga = q.Harga,
                                                   HargaOTR = q.HargaOtr,
                                                   NamaBarang = q.NamaBarang,
                                                   MerkID = q.MerkID,
                                                   ModelBarangID = q.ModelBarangID,
                                                   TypeBarangID = q.TypeBarangID,
                                                   WarnaID = q.WarnaID,
                                                   BarangID = q.barangID,
                                                   UserProfileID = q.UserProfileId

                                               }

                                        ).Skip(startRec).Take(pageSize).ToListAsync();

                    response.draw = Convert.ToInt32(draw);
                    response.recordsTotal = totalRecords;
                    response.recordsFiltered = recFilter;
                    response.Message = "Success";
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }

            }
            return response;
        }
    }
}
