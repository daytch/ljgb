using ljgb.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using ljgb.DataAccess.Interface;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.Common.ViewModel;

namespace ljgb.DataAccess.Repository
{
    public class TypeBarangRepository :ITypeBarang
    {
        ljgbContext db;
        public TypeBarangRepository(ljgbContext _db)
        {
            db = _db;
        }

        public async Task<List<TypeBarang>> GetAllType()
        {
            List<TypeBarang> Result = new List<TypeBarang>();
            if (db != null)
            {
                Result = await db.TypeBarang.Where(x => x.RowStatus == true).ToListAsync();
            }
            return Result;
        }

        public async Task<long> Add(TypeBarang model)
        {
            if (db != null)
            {
                await db.TypeBarang.AddAsync(model);
                await db.SaveChangesAsync();

                return model.Id;
            }

            return 0;
        }

        public async Task<TypeBarangResponse> AddPost(TypeBarang model)
        {
            TypeBarangResponse response = new TypeBarangResponse();
            try
            {
                if (db != null)
                {
                  

                    await db.TypeBarang.AddAsync(model);
                    await db.SaveChangesAsync();

                    response.Message = "Data Already Saved!";
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

        public async Task<TypeBarangResponse> DeletePost(TypeBarangRequest request)
        {
            TypeBarangResponse response = new TypeBarangResponse();
            try
            {
                if (db != null)
                {
                    //Find the warna for specific userprofile
                    TypeBarang model = await db.TypeBarang.FirstOrDefaultAsync(x => x.Id == request.ID);

                    if (model != null)
                    {
                        model.Modified = DateTime.Now;
                        model.ModifiedBy = request.UserName;
                        model.RowStatus = false;

                        //Commit the transaction
                        await db.SaveChangesAsync();
                        response.Message = "Data Already Saved";
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.Message = "Data Not Found";
                        response.IsSuccess = false;
                    }
                    
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

        public async Task<TypeBarangResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            TypeBarangResponse response = new TypeBarangResponse();
            try
            {
                if (db != null)
                {
                    var query = (from type in db.TypeBarang
                                 join model in db.ModelBarang
                                 on type.ModelBarangId equals model.Id
                                 join merk in db.Merk
                                 on model.MerkId equals merk.Id
                                 where merk.RowStatus == true
                                 && model.RowStatus == true
                                 select new
                                 {
                                     type.Id,
                                     type.Name,
                                     namaModelBarang = model.Name,
                                     type.ModelBarangId,
                                     NamaMerk = merk.Name,
                                     model.MerkId,
                                     type.Description,
                                     type.Created,
                                     type.CreatedBy,
                                     type.Modified,
                                     type.ModifiedBy,
                                     type.RowStatus

                                 }
                                );

                    int totalRecords = query.Count();
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        query = query.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                                            p.namaModelBarang.ToString().ToLower().Contains(search.ToLower()) ||
                                            p.NamaMerk.ToString().ToLower().Contains(search.ToLower()) ||
                                            p.Description.ToLower().Contains(search.ToLower()));
                    }
                    int recFilter = query.Count();
                    response.ListModel =  await (from model in query
                                  where model.RowStatus == true
                                  select new TypeBarangViewModel
                                  {
                                      ID = model.Id,
                                      Name = model.Name,
                                      NamaMerk = model.NamaMerk,
                                      NamaModelBarang = model.namaModelBarang,
                                      MerkID = model.MerkId,
                                      ModelBarangID = model.ModelBarangId,
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

                //return await (from model in db.TypeBarang
                //              where model.RowStatus == true
                //              select new TypeBarangViewModel
                //              {
                //                  ID = model.Id,
                //                  Nama = model.Name,
                //                  ModelBarangID = model.ModelBarangId,
                //                  Description = model.Description,
                //                  Created = model.Created,
                //                  CreatedBy = model.CreatedBy,
                //                  Modified = model.Modified,
                //                  ModifiedBy = model.ModifiedBy,
                //                  RowStatus = model.RowStatus
                //              }).ToListAsync();

            }

            return response;
        }

        public async Task<TypeBarangResponse> GetAllWithModelID(TypeBarangRequest request)
        {
            TypeBarangResponse response = new TypeBarangResponse();
            try
            {

                if (db != null)
                {
                    response.ListModel = await(from model in db.TypeBarang
                                           where model.ModelBarangId == request.ModelBarangID && model.RowStatus == true
                                           select new TypeBarangViewModel
                                           {
                                               ID = model.Id,
                                               Name = model.Name,
                                               ModelBarangID = model.ModelBarangId,
                                               Description = model.Description,
                                               Created = model.Created,
                                               CreatedBy = model.CreatedBy,
                                               Modified = model.Modified,
                                               ModifiedBy = model.ModifiedBy,
                                               RowStatus = model.RowStatus
                                           }).ToListAsync();
                    if (response.ListModel == null)
                    {
                        response.IsSuccess = true;
                        response.Message = "Data Not Found";
                    }
                    response.IsSuccess = true;
                    response.Message = "Load Success";
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

                //return await (from model in db.TypeBarang
                //              where model.Id == ID && model.RowStatus == true
                //              select new TypeBarangViewModel
                //              {
                //                  ID = model.Id,
                //                  Nama = model.Name,
                //                  ModelBarangID = model.ModelBarangId,
                //                  Description = model.Description,
                //                  Created = model.Created,
                //                  CreatedBy = model.CreatedBy,
                //                  Modified = model.Modified,
                //                  ModifiedBy = model.ModifiedBy,
                //                  RowStatus = model.RowStatus
                //              }).FirstOrDefaultAsync();

            }

            return response;
        }

        public async Task<TypeBarangResponse> GetPost(TypeBarangRequest request)
        {
            TypeBarangResponse response = new TypeBarangResponse();
            try
            {

                if (db != null)
                {
                    response.Model =  await (from model in db.TypeBarang
                                  where model.Id == request.ID && model.RowStatus == true
                                  select new TypeBarangViewModel
                                  {
                                      ID = model.Id,
                                      Name = model.Name,
                                      ModelBarangID = model.ModelBarangId,
                                      Description = model.Description,
                                      Created = model.Created,
                                      CreatedBy = model.CreatedBy,
                                      Modified = model.Modified,
                                      ModifiedBy = model.ModifiedBy,
                                      RowStatus = model.RowStatus
                                  }).FirstOrDefaultAsync();
                    if (response.Model == null)
                    {
                        response.IsSuccess = true;
                        response.Message = "Data Not Found";
                    }
                    response.IsSuccess = true;
                    response.Message = "Load Success";
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

                //return await (from model in db.TypeBarang
                //              where model.Id == ID && model.RowStatus == true
                //              select new TypeBarangViewModel
                //              {
                //                  ID = model.Id,
                //                  Nama = model.Name,
                //                  ModelBarangID = model.ModelBarangId,
                //                  Description = model.Description,
                //                  Created = model.Created,
                //                  CreatedBy = model.CreatedBy,
                //                  Modified = model.Modified,
                //                  ModifiedBy = model.ModifiedBy,
                //                  RowStatus = model.RowStatus
                //              }).FirstOrDefaultAsync();

            }

            return response;
        }

        public async Task<List<SP_TypeByKotaIDMerkIDModelID>> GetTypeByKotaIDMerkIDModelID(TypeBarangRequest request)
        {
            if (db != null)
            {
                try
                {
                    return await db.Set<SP_TypeByKotaIDMerkIDModelID>().FromSql("EXEC sp_TypeByKotaIDMerkIDModelID {0},{1},{2}",
                        request.KotaID, request.MerkID, request.ModelBarangID).AsNoTracking().ToListAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return null;
        }

        public async Task<TypeBarang> GetTypeByName(TypeBarang request)
        {
            TypeBarang result = new TypeBarang();

            try
            {
                result = await db.TypeBarang.Where(x => x.RowStatus == true && x.Name.ToLower().Equals(request.Name)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        public async Task<TypeBarangResponse> UpdatePost(TypeBarangRequest request)
        {
            TypeBarangResponse response = new TypeBarangResponse();

            try
            {
                if (db != null)
                {
                    TypeBarang model = await db.TypeBarang.Where(x => x.RowStatus == true && x.Id == request.ID).FirstAsync();
                    model.ModelBarangId = request.ModelBarangID;
                    model.Name = request.Name;
                    model.Description = request.Description;
                    model.Modified = DateTime.Now;
                    model.ModifiedBy = "xsivicto1905";

                    db.TypeBarang.Update(model);

                    //Commit the transaction
                    await db.SaveChangesAsync();
                        
                  
                    response.Message = "Data Already Saved";
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
    }
}
