using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using ljgb.Common.Responses;
using ljgb.Common.ViewModel;
using ljgb.Common.Requests;

namespace ljgb.DataAccess.Repository
{
    public class MerkRepository : IMerk
    {

        ljgbContext db;
        public MerkRepository(ljgbContext _db)
        {
            db = _db;
        }

        public async Task<List<Merk>> GetAllMerk()
        {
            List<Merk> Result = new List<Merk>();
            if (db!=null)
            {
                Result = await db.Merk.Where(x => x.RowStatus == true).ToListAsync();
            }
            return Result;
        }

        public async Task<long> Add(Merk merk)
        {
            if (db != null)
            {
                await db.Merk.AddAsync(merk);
                await db.SaveChangesAsync();

                return merk.Id;
            }

            return 0;
        }

        public async Task<MerkResponse> AddPost(MerkRequest request)
        {
            MerkResponse respose = new MerkResponse();
            if (db != null)
            {
                try
                {
                    Merk model = new Merk();
                    model.Name = request.Name;
                    model.Description = request.Description;
                    model.Created = DateTime.Now;
                    model.CreatedBy = request.UserName;
                    model.RowStatus = true;
                    await db.Merk.AddAsync(model);

                    await db.SaveChangesAsync();
                    respose.Message = "Data Already saved";
                    respose.IsSuccess = true;

                }
                catch (Exception ex)
                {
                    respose.Message = ex.ToString();
                    respose.IsSuccess = false;

                }
            }

            return respose;
        }

        public async Task<MerkResponse> DeletePost(long ID, string username)
        {
            MerkResponse response = new MerkResponse();

            if (db != null)
            {
                try
                {
                    //Find the warna for specific userprofile
                    var model = await db.Merk.FirstOrDefaultAsync(x => x.Id == ID);

                    if (model != null)
                    {
                        model.Modified = DateTime.Now;
                        model.ModifiedBy = username;
                        model.RowStatus = false;
                        //Delete that warna
                        db.Merk.Update(model);

                        //Commit the transaction
                        await db.SaveChangesAsync();

                        response.Message = "Data Already Deleted";
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.Message = "Data not Found";
                        response.IsSuccess = false;
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

        public async Task<MerkResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {

            MerkResponse response = new MerkResponse();
            if (db != null)
            {
                try
                {
                    var query = (from model in db.Merk
                                 where model.RowStatus == true
                                 select model
                                  );
                    //response.ListModel = await (from model in db.Merk
                    //              where model.RowStatus == true
                    //              select new MerkViewModel
                    //              {
                    //                  ID = model.Id,
                    //                  Nama = model.Nama,
                    //                  Description = model.Description,
                    //                  Created = model.Created,
                    //                  CreatedBy = model.CreatedBy,
                    //                  Modified = model.Modified,
                    //                  ModifiedBy = model.ModifiedBy,
                    //                  RowStatus = model.RowStatus
                    //              }).ToListAsync();
                    int totalRecords = query.Count();
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        query = query.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                                    p.Description.ToLower().Contains(search.ToLower()));
                    }
                    int recFilter = query.Count();

                    response.ListModel = await (from model in query

                                                select new MerkViewModel
                                                {
                                                    ID = model.Id,
                                                    Name = model.Name,
                                                    Description = model.Description,
                                                    Created = model.Created,
                                                    CreatedBy = model.CreatedBy,
                                                    Modified = model.Modified,
                                                    ModifiedBy = model.ModifiedBy,
                                                    RowStatus = model.RowStatus
                                                }).Skip(startRec).Take(pageSize).ToListAsync();

                    response.draw = Convert.ToInt32(draw);
                    response.recordsTotal = totalRecords;
                    response.recordsFiltered = recFilter;
                    response.Message = "Success";
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = ex.ToString();
                }
            }

            return response;
        }

        public async Task<MerkResponse> GetAllWithoutFilter()
        {
            MerkResponse response = new MerkResponse();
            if (db != null)
            {
                try
                {
                    response.ListModel = await (from model in db.Merk
                                                where model.RowStatus == true
                                                select new MerkViewModel
                                                {
                                                    ID = model.Id,
                                                    Name = model.Name,
                                                    Description = model.Description,
                                                    Created = model.Created,
                                                    CreatedBy = model.CreatedBy,
                                                    Modified = model.Modified,
                                                    ModifiedBy = model.ModifiedBy,
                                                    RowStatus = model.RowStatus
                                                }).ToListAsync();

                    //response.ListModel = await (from model in db.Merk
                    //              where model.RowStatus == true
                    //              select new MerkViewModel
                    //              {
                    //                  ID = model.Id,
                    //                  Nama = model.Nama,
                    //                  Description = model.Description,
                    //                  Created = model.Created,
                    //                  CreatedBy = model.CreatedBy,
                    //                  Modified = model.Modified,
                    //                  ModifiedBy = model.ModifiedBy,
                    //                  RowStatus = model.RowStatus
                    //              }).ToListAsync();

                    response.Message = "Success";
                    response.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = ex.ToString();
                }
            }

            return response;
        }

        public async Task<MerkResponse> GetPost(long ID)
        {
            MerkResponse response = new MerkResponse();
            if (db != null)
            {
                try
                {
                    response.Model = await (from model in db.Merk
                                            where model.Id == ID && model.RowStatus == true
                                            select new MerkViewModel
                                            {
                                                ID = model.Id,
                                                Name = model.Name,
                                                Description = model.Description,
                                                Created = model.Created,
                                                CreatedBy = model.CreatedBy,
                                                Modified = model.Modified,
                                                ModifiedBy = model.ModifiedBy,
                                                RowStatus = model.RowStatus
                                            }).FirstOrDefaultAsync();
                    response.IsSuccess = true;
                    response.Message = "Load Success";
                }
                catch (Exception ex)
                {

                    response.IsSuccess = false;
                    response.Message = ex.ToString();
                }
            }

            return response;
        }

        public async Task<MerkResponse> UpdatePost(MerkRequest request)
        {
            MerkResponse response = new MerkResponse();
            if (db != null)
            {
                try
                {

                    Merk model = await db.Merk.Where(x => x.Id == request.ID).FirstAsync();
                    model.Modified = DateTime.Now;
                    model.ModifiedBy = request.UserName;
                    model.Name = request.Name;
                    model.Description = request.Description;
                    //Delete that warna
                    db.Merk.Update(model);

                    //Commit the transaction
                    await db.SaveChangesAsync();
                    response.Message = "Data Already Updated";
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

        public List<SP_MerkByKotaID> GetMerkByKotaID(long KotaID)
        {
            if (db != null)
            {
                try
                {
                    return db.Set<SP_MerkByKotaID>().FromSql("EXEC sp_MerkByKotaID {0}",
                        KotaID).AsNoTracking().ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return null;
        }

        public async Task<Merk> GetMerkByName(string request)
        {
            try
            {
                return await db.Merk.Where(x => x.RowStatus == true && x.Name.ToLower() == request.ToLower()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
