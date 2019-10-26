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

                    negoBarang.UserProfileId = model.UserProfileID;
                    negoBarang.BarangId = model.BarangID;
                    negoBarang.TypePenawaran = model.TypePenawaran;
                    negoBarang.Harga = model.Harga;
                    negoBarang.Created = DateTime.Now;
                    negoBarang.CreatedBy = model.CreatedBy;
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
                    response.ListModel = await(from model in db.NegoBarang
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
    }
}
