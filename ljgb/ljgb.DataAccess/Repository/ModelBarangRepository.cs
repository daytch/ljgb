using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using ljgb.Common.Responses;
using ljgb.Common.Requests;
using ljgb.Common.ViewModel;

namespace ljgb.DataAccess.Repository
{
    public class ModelBarangRepository : IModelBarang
    {
        ljgbContext db;
        public ModelBarangRepository(ljgbContext _db)
        {
            db = _db;
        }
        public async Task<ModelBarangResponse> AddPost(ModelBarangRequest request)
        {
            ModelBarangResponse response = new ModelBarangResponse();
            try
            {
                if (db != null)
                {
                    ModelBarang model = new ModelBarang();
                    model.Nama = request.Nama;
                    model.Description = request.Description;
                    model.MerkId = request.MerkID;
                    model.Created = DateTime.Now;
                    model.Createdby = "xsivicto1905";
                    model.RowStatus = true;

                    await db.ModelBarang.AddAsync(model);
                    await db.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Message = "Data Already Saved";
                }
                else
                {

                    response.Message ="Opps, Something Error with System Righ Now !";
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

        public async Task<ModelBarangResponse> DeletePost(ModelBarangRequest request)
        {
            ModelBarangResponse response = new ModelBarangResponse();
            

            try
            {
                if (db != null)
                {
                    //Find the warna for specific userprofile
                    ModelBarang model = await db.ModelBarang.FirstOrDefaultAsync(x => x.Id == request.ID);

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
                        response.Message = "Data Not Found!";
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

        public async Task<ModelBarangResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            ModelBarangResponse response = new ModelBarangResponse();
            try
            {
              
                if (db != null)
                {
                    var query = (from model in db.ModelBarang
                                 join merk in db.Merk
                                 on model.MerkId equals merk.Id
                                 where merk.RowStatus == true
                                 && model.RowStatus == true
                                 select new
                                 {
                                     model.Id,
                                     model.Nama,
                                     NamaMerk = merk.Nama,
                                     model.MerkId,
                                     model.Description,
                                     model.Created,
                                     model.Createdby,
                                     model.Modified,
                                     model.ModifiedBy,
                                     model.RowStatus

                                 }
                                );

                    int totalRecords = query.Count();
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        query = query.Where(p => p.Nama.ToString().ToLower().Contains(search.ToLower()) ||
                                            p.NamaMerk.ToString().ToLower().Contains(search.ToLower())||
                                            p.Description.ToLower().Contains(search.ToLower()));
                    }
                    int recFilter = query.Count();
                    response.ListModel = await (from model in query
                                                
                                                select new ModelBarangViewModel
                                                {
                                                    ID = model.Id,
                                                    Nama = model.Nama,
                                                    NamaMerk = model.NamaMerk,
                                                    MerkID = model.MerkId,
                                                    Description = model.Description,
                                                    Created = model.Created,
                                                    CreatedBy = model.Createdby,
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

        public async Task<ModelBarangResponse> GetModelWithMerkID(ModelBarangRequest request)
        {
            ModelBarangResponse response = new ModelBarangResponse();
            try
            {
                if (db != null)
                {
                    response.ListModel = await(from model in db.ModelBarang
                                           where model.MerkId == request.MerkID & model.RowStatus == true
                                           select new ModelBarangViewModel
                                           {
                                               ID = model.Id,
                                               Nama = model.Nama,
                                               MerkID = model.MerkId,
                                               Description = model.Description,
                                               Created = model.Created,
                                               CreatedBy = model.Createdby,
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

                response.Message = ex.ToString();
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<ModelBarangResponse> GetPost(long ID)
        {
            ModelBarangResponse response = new ModelBarangResponse();
            try
            {
                if (db != null)
                {
                    response.Model =  await (from model in db.ModelBarang
                                  where model.Id == ID & model.RowStatus == true
                                  select new ModelBarangViewModel
                                  {
                                      ID = model.Id,
                                      Nama = model.Nama,
                                      MerkID = model.MerkId,
                                      Description = model.Description,
                                      Created = model.Created,
                                      CreatedBy = model.Createdby,
                                      Modified = model.Modified,
                                      ModifiedBy = model.ModifiedBy,
                                      RowStatus = model.RowStatus
                                  }).FirstOrDefaultAsync();
                    response.Message = "Load Success";
                    response.IsSuccess = false;
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

        public async Task<ModelBarangResponse> UpdatePost(ModelBarangRequest request)
        {
            ModelBarangResponse response = new ModelBarangResponse();
            try
            {

                if (db != null)
                {
                    ModelBarang model = await db.ModelBarang.Where(x=>x.Id == request.ID).FirstAsync();
                    model.Modified = DateTime.Now;
                    model.ModifiedBy = "xsivicto1905";
                    model.MerkId = request.MerkID;
                    model.Nama = request.Nama;
                    model.Description = request.Description;

                    db.ModelBarang.Update(model);

                    //Commit the transaction
                    await db.SaveChangesAsync();
                    

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
