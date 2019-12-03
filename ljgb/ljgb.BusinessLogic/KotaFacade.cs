
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.Common.ViewModel;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.BusinessLogic
{
    public class KotaFacade
    {
        #region Important
        private ljgbContext db;
        private IKota dep;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public KotaFacade()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            db = new ljgbContext(optionsBuilder.Options);
            this.dep = new KotaRepository(db);

        }
        #endregion

        public async Task<KotaResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            var models = await dep.GetAll(search, order, orderDir, startRec, pageSize, draw);

            if (models == null)
            {
                return null;
            }
            return models;
        }

        public async Task<List<Dropdown>> GetAllForDropdown(int ProvinsiID)
        {
            List<Kota> ListKota = new List<Kota>();
            if (ProvinsiID < 1)
            {
                ListKota = await dep.GetAll();
            }
            else
            {
                ListKota = await dep.GetKotaByProvinsiID(ProvinsiID);
            }
            List<Dropdown> ListDropdown = ListKota.Select(x => new Dropdown() { ID = x.Id, Text = x.Name }).ToList();
            return ListDropdown;
        }



        public async Task<KotaResponse> GetPost(KotaRequest req)
        {
            KotaResponse response = new KotaResponse();
               var model = await dep.GetPost(req);

            if (model == null)
            {
                return null;
            }
            return response;

        }

        public async Task<KotaResponse> AddPost(KotaRequest req)
        {
            KotaResponse response = new KotaResponse();
            try
            {
                Kota model = new Kota();
                model.ProvinsiId = req.ProvinsiID;
                model.Name = req.Name;
                model.Description = req.Description;
                model.Created = DateTime.Now;
                model.CreatedBy = req.UserName;
                model.RowStatus = true;
                await dep.AddPost(model);
                response.Message = "Success";
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.Message = "Failed";
                response.IsSuccess = false;
            }
            return response;

        }

        public async Task<KotaResponse> DeletePost(KotaRequest req)
        {

            return await dep.DeletePost(req);

        }

        public async Task<KotaResponse> UpdatePost(KotaRequest req)
        {
            KotaResponse response = new KotaResponse();
            try
            {
                Kota model = new Kota();
                model = await dep.GetPost(req);

               
                model.ProvinsiId = req.ProvinsiID;
                model.Name = req.Name;

                model.Description = req.Description;
                model.Modified = DateTime.Now;
                model.ModifiedBy = req.UserName;
                model.RowStatus = true;
                await dep.UpdatePost(model);

                var result = await dep.UpdatePost(model);
                if (result)
                {
                    response.Message = "Success";
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = "Failed";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                response.Message = "Failed";
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<KotaResponse> GetAllWithoutFilter()
        {
            KotaResponse response = new KotaResponse();

            var listKota = await dep.GetAllWithoutFilter();
            response.ListKota = (from kota in listKota
                                 select new KotaViewModel
                                 {
                                     ID = kota.Id,
                                     Name = kota.Name
                                 }).ToList();
            return response;
        }
    }
}
