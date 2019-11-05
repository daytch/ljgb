using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ljgb.Common.ViewModel;

namespace ljgb.DataAccess.Repository
{
    public class ProvinsiRepository : IProvinsi
    {
        ljgbContext db;
        public ProvinsiRepository(ljgbContext _db)
        {
            db = _db;
        }
        public async Task<ProvinsiResponse> AddPost(ProvinsiRequest request)
        {
            ProvinsiResponse response = new ProvinsiResponse();


            if (db != null)
            {
                try
                {

                    Provinsi model = new Provinsi();

                    model.Nama = request.Nama;
                    model.Description = request.Description;
                    model.Created = DateTime.Now;
                    model.CreatedBy = "xsivicto1905";
                    model.RowStatus = true;

                    await db.Provinsi.AddAsync(model);
                    await db.SaveChangesAsync();

                    response.Message = "Data has been saved";
                }
                catch (Exception ex)
                {

                    response.IsSuccess = false;
                    response.Message = ex.ToString();
                }
            }


            return response;
        }

        public async Task<ProvinsiResponse> DeletePost(ProvinsiRequest request)
        {
            ProvinsiResponse response = new ProvinsiResponse();

            if (db != null)
            {
                try
                {
                    Provinsi model = await db.Provinsi.Where(x => x.RowStatus == true && x.Id == request.ID).FirstOrDefaultAsync();
                    if (model != null)
                    {
                        model.RowStatus = false;
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

        public async Task<ProvinsiResponse> GetAll()
        {
            ProvinsiResponse response = new ProvinsiResponse();

            if (db != null)
            {
                try
                {
                    response.ListProvinsi = await (from prov in db.Provinsi
                                                   where prov.RowStatus == true
                                                   select new ProvinsiViewModel
                                                   {
                                                       ID = prov.Id,
                                                       Nama = prov.Nama,
                                                       Created = prov.Created,
                                                       CreatedBy = prov.CreatedBy,
                                                       Description = prov.Description,
                                                       Modified = prov.Modified,
                                                       ModifiedBy = prov.ModifiedBy,
                                                       RowStatus = prov.RowStatus
                                                   }).ToListAsync();

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

        public async Task<List<Provinsi>> GetAllForDropdown()
        {
            List<Provinsi> response = new List<Provinsi>();

            if (db != null)
            {
                try
                {
                    response = await db.Provinsi.Where(x => x.RowStatus == true).Select(x => new Provinsi() { Id = x.Id, Nama = x.Nama }).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return response;
        }

        public async Task<ProvinsiResponse> GetPost(ProvinsiRequest request)
        {
            ProvinsiResponse response = new ProvinsiResponse();

            if (db != null)
            {
                try
                {
                    response.Model = await(from prov in db.Provinsi
                                                  where prov.RowStatus == true && request.ID == prov.Id
                                                  select new ProvinsiViewModel
                                                  {
                                                      ID = prov.Id,
                                                      Nama = prov.Nama,
                                                      Created = prov.Created,
                                                      CreatedBy = prov.CreatedBy,
                                                      Description = prov.Description,
                                                      Modified = prov.Modified,
                                                      ModifiedBy = prov.ModifiedBy,
                                                      RowStatus = prov.RowStatus
                                                  }).FirstOrDefaultAsync();

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

        public async Task<ProvinsiResponse> UpdatePost(ProvinsiRequest request)
        {

            ProvinsiResponse response = new ProvinsiResponse();


            if (db != null)
            {
                try
                {

                    Provinsi model = await db.Provinsi.Where(x => x.RowStatus == true && x.Id == request.ID).FirstOrDefaultAsync();

                    model.Nama = request.Nama;
                    model.Description = request.Description;
                    model.Modified = DateTime.Now;
                    model.ModifiedBy = "xsivicto1905" ;
                   

                  
                    await db.SaveChangesAsync();

                    response.Message = "Data has been saved";
                }
                catch (Exception ex)
                {

                    response.IsSuccess = false;
                    response.Message = ex.ToString();
                }
            }


            return response;
        }
    }
}
