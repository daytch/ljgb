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
                    response = await db.Kota.Where(x => x.RowStatus == true && x.ProvinsiId == ProvinsiID).Select(x => new Kota() { Id = x.Id, Name = x.Name }).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return response;
        }

        public async Task<long> AddPost(Kota request)
        {
            long result = 0;

            if (db != null)
            {
                try
                {     
                    await db.Kota.AddAsync(request);
                    result = await db.SaveChangesAsync();               

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return result;
        }

        public async Task<KotaResponse> DeletePost(KotaRequest request)
        {
            KotaResponse response = new KotaResponse();

            if (db != null)
            {
                try
                {
                    Kota model =  await db.Kota.Where(x => x.RowStatus == true && x.Id == request.ID).FirstOrDefaultAsync();
                    if (model != null)
                    {
                        model.Modified = DateTime.Now;
                        model.ModifiedBy = request.UserName;
                        model.RowStatus = false;
                        await db.SaveChangesAsync();
                        response.IsSuccess = true;
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
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return response;
        }

        public async Task<KotaResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            KotaResponse response = new KotaResponse();

            if (db != null)
            {
                try
                {
                    var query = (from kota in db.Kota
                                 join provinsi in db.Provinsi
                                 on kota.ProvinsiId equals provinsi.Id
                                 where kota.RowStatus == true && provinsi.RowStatus == true
                                 select new
                                 {
                                     ID = kota.Id,
                                     Name = kota.Name,
                                     kota.Description,
                                     ProvinsiID = kota.ProvinsiId,
                                     ProvinsiName = provinsi.Name
                                 }
                               );
                    int totalRecords = query.Count();
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        query = query.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                                    p.ProvinsiName.ToLower().Contains(search.ToLower()) ||
                                    p.Description.ToLower().Contains(search.ToLower()));
                    }
                    int recFilter = query.Count();

                    response.ListKota = await (from model in query
                                               select new KotaViewModel
                                               {
                                                   ID = model.ID,
                                                   Name = model.Name,
                                                   Description = model.Description,
                                                   ProvinsiID = model.ProvinsiID,
                                                   ProvinsiName = model.ProvinsiName,
                                               }).Skip(startRec).Take(pageSize).ToListAsync();

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
                    response.Message = "Success";
                    response.draw = Convert.ToInt32(draw);
                    response.recordsTotal = totalRecords;
                    response.recordsFiltered = recFilter;
                    response.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return response;
        }

        public async Task<Kota> GetPost(KotaRequest request)
        {
            Kota response = new Kota();
            if (db != null)
            {
                try
                {
                    return await (from model in db.Kota
                                  where model.RowStatus == true && model.Id == request.ID
                                  select model).FirstOrDefaultAsync();
                                               
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }

            return response;
        }

        public async Task<bool> UpdatePost(Kota request)
        {

            bool result = false;


            if (db != null)
            {
                try
                {




                    db.Kota.Update(request);
                    await db.SaveChangesAsync();
                    result = true;
                  
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }


            return result;
        }

        public Task<List<Kota>> GetAllWithoutFilter()
        {

            if (db!=null)
            {
                try
                {
                    return db.Kota.Where(x => x.RowStatus == true).ToListAsync();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return null;
        }
    }
}
