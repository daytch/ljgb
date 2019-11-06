using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Interface;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ljgb.Common.ViewModel;
using ljgb.DataAccess.Model;

namespace ljgb.DataAccess.Repository
{
    public class KotaRepository : IKota
    {
        ljgbContext db;
        public KotaRepository(ljgbContext _db)
        {
            db = _db;
        }

        public async Task<List<Kota>> GetKotaByProvinsiID(int ProvinsiID)
        {
            List<Kota> response = new List<Kota>();

            if (db != null)
            {
                try
                {
                    response = await db.Kota.Where(x => x.RowStatus == true && x.ProvinsiId == ProvinsiID).Select(x => new Kota() { Id = x.Id, Nama = x.Nama }).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return response;
        }

        public async Task<KotaResponse> AddPost(KotaRequest request)
        {
            KotaResponse response = new KotaResponse();


            if (db != null)
            {
                try
                {

                    Kota model = new Kota();

                    model.Name = request.Name;
                    model.ProvinsiId = request.ProvinsiID;
                    model.Created = DateTime.Now;
                    model.CreatedBy = "xsivicto1905";
                    model.RowStatus = true;

                    //await db.Kota.AddAsync(model);
                    await db.SaveChangesAsync();
                    response.Message = "Data has been Saved";

                }
                catch (Exception ex)
                {

                    response.IsSuccess = false;
                    response.Message = ex.ToString();
                }
            }


            return response;
        }

        public async Task<KotaResponse> DeletePost(KotaRequest request)
        {
            KotaResponse response = new KotaResponse();

            if (db != null)
            {
                try
                {
                    Kota model = new Kota(); // await db.Kota.Where(x => x.RowStatus == true && x.Id == request.ID).FirstOrDefaultAsync();
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

        public async Task<List<Kota>> GetAll()
        {
            List<Kota> response = new List<Kota>();
            if (db != null)
            {
                try
                {
                    response = await db.Kota.Where(x => x.RowStatus == true).ToListAsync();
                    #region Old
                    //response.ListKota = await(from model in db.Kota
                    //                           where model.RowStatus == true
                    //                           select new KotaViewModel
                    //                           {
                    //                               ID = model.Id,
                    //                               Nama = model.Nama,
                    //                               Description = model.Description,
                    //                               ProvinsiID = model.ProvinsiId,
                    //                               Created = model.Created,
                    //                               CreatedBy = model.CreatedBy,
                    //                               Modified = model.Modified,
                    //                               ModifiedBy = model.ModifiedBy,
                    //                               RowStatus = model.RowStatus
                    //                           }).ToListAsync();
                    //response.ListProvinsi = await (from prov in db.Provinsi
                    //                               where prov.RowStatus == true
                    //                               select new ProvinsiViewModel
                    //                               {
                    //                                   ID = prov.Id,
                    //                                   Nama = prov.Nama,
                    //                                   Created = prov.Created,
                    //                                   CreatedBy = prov.CreatedBy,
                    //                                   Description = prov.Description,
                    //                                   Modified = prov.Modified,
                    //                                   ModifiedBy = prov.ModifiedBy,
                    //                                   RowStatus = prov.RowStatus
                    //                               }).ToListAsync();
                    //response.Message = "Success";
                    #endregion
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return response;
        }

        public async Task<KotaResponse> GetPost(KotaRequest request)
        {
            KotaResponse response = new KotaResponse();
            if (db != null)
            {
                try
                {
                    response.ListKota = await (from model in db.Kota
                                               where model.RowStatus == true && model.Id == request.ID
                                               select new KotaViewModel
                                               {
                                                   ID = model.Id,
                                                   Nama = model.Nama,
                                                   Description = model.Description,
                                                   ProvinsiID = model.ProvinsiId,
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

        public async Task<KotaResponse> UpdatePost(KotaRequest request)
        {

            KotaResponse response = new KotaResponse();


            if (db != null)
            {
                try
                {

                    Kota model = new Kota(); //await db.Kota.Where(x => x.RowStatus == true && x.Id == request.ID).FirstOrDefaultAsync();

                    model.Name = request.Name;
                    model.Description = request.Description;
                    model.Modified = DateTime.Now;
                    model.ModifiedBy = "xsivicto1905";



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
