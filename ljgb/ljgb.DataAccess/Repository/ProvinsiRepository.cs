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
        public async Task<long> AddPost(Provinsi request)
        {
            long result = 0;


            if (db != null)
            {
                try
                {

                  

                    await db.Provinsi.AddAsync(request);
                    result = await db.SaveChangesAsync();

                   
                }
                catch (Exception ex)
                {

                    result = 0;
                }
            }


            return result;
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

        public async Task<ProvinsiResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            ProvinsiResponse response = new ProvinsiResponse();

            if (db != null)
            {
                try
                {
                    var query = (from model in db.Provinsi
                                 where model.RowStatus == true
                                 select model
                                 );
                    int totalRecords = query.Count();
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        query = query.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                                    p.Description.ToLower().Contains(search.ToLower()));
                    }
                    int recFilter = query.Count();

                    response.ListProvinsi = await (from prov in query
                                                   select new ProvinsiViewModel
                                                   {
                                                       ID = prov.Id,
                                                       Name = prov.Name,
                                                       Created = prov.Created,
                                                       CreatedBy = prov.CreatedBy,
                                                       Description = prov.Description,
                                                       Modified = prov.Modified,
                                                       ModifiedBy = prov.ModifiedBy,
                                                       RowStatus = prov.RowStatus
                                                   }).Skip(startRec).Take(pageSize).ToListAsync();

                    response.Message = "Success";
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
                                                      Name = prov.Name,
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

        public async Task<Provinsi> GetPostByID(long ID)
        {
            Provinsi response = new Provinsi();
            if (db != null)
            {
                try
                {
                   response = await db.Provinsi.Where(x=>x.RowStatus == true && x.Id == ID).FirstOrDefaultAsync();

                  
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            return response;
        }
        public async Task<long> UpdatePost(Provinsi request)
        {

           
            long result = 0;

            if (db != null)
            {
                try
                {
                    
                    


                    db.Provinsi.Update(request);
                    result = await db.SaveChangesAsync();

                   
                }
                catch (Exception ex)
                {

                    result = 0;
                }
            }


            return result;
        }
    }
}
