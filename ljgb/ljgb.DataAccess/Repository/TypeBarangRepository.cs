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
                        model.RowStatus = false;

                        //Commit the transaction
                        db.SaveChangesAsync();
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
                                     type.Nama,
                                     namaModelBarang = model.Nama,
                                     type.ModelBarangId,
                                     NamaMerk = merk.Nama,
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
                        query = query.Where(p => p.Nama.ToString().ToLower().Contains(search.ToLower()) ||
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
                                      Nama = model.Nama,
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
                                  }).ToListAsync();
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
<<<<<<< HEAD

                response.Message = ex.ToString();
                response.IsSuccess = false;
=======
                return await (from model in db.TypeBarang
                              where model.RowStatus == true
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
>>>>>>> 1ad68382c47205fb378ded98b05f8f688d92ea2f
            }

            return response;
        }

        public async Task<TypeBarangResponse> GetPost(TypeBarangRequest request)
        {
            TypeBarangResponse response = new TypeBarangResponse();
            try
            {
<<<<<<< HEAD
                if (db != null)
                {
                    response.Model =  await (from model in db.TypeBarang
                                  where model.Id == request.ID && model.RowStatus == true
                                  select new TypeBarangViewModel
                                  {
                                      ID = model.Id,
                                      Nama = model.Nama,
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
=======
                return await (from model in db.TypeBarang
                              where model.Id == ID && model.RowStatus == true
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
>>>>>>> 1ad68382c47205fb378ded98b05f8f688d92ea2f
            }

            return response;
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
                    model.Nama = request.Nama;
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
