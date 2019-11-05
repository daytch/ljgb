
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

        //public async Task<KotaResponse> GetAll()
        //{
        //    var models = await dep.GetAll();

        //    if (models == null)
        //    {
        //        return null;
        //    }
        //    return models;
        //}

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
            List<Dropdown> ListDropdown = ListKota.Select(x => new Dropdown() { ID = x.Id, Text = x.Nama }).ToList();
            if (ListDropdown == null)
            {
                return null;
            }
            return ListDropdown;
        }

        public async Task<KotaResponse> GetPost(KotaRequest req)
        {
            var model = await dep.GetPost(req);

            if (model == null)
            {
                return null;
            }
            return model;

        }

        public async Task<KotaResponse> AddPost(KotaRequest req)
        {
            return await dep.AddPost(req);

        }

        public async Task<KotaResponse> DeletePost(KotaRequest req)
        {

            return await dep.DeletePost(req);

        }

        public async Task<KotaResponse> UpdatePost(KotaRequest req)
        {
            return await dep.UpdatePost(req);
        }
    }
}
