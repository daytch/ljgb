using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.BusinessLogic
{
    public class NegoBarangFacade
    {
        #region Important
        private ljgbContext db;
        private INegoBarang dep;

        public NegoBarangFacade()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            db = new ljgbContext(optionsBuilder.Options);
            this.dep = new NegoBarangRepository(db);
        }
        #endregion

        public async Task<NegoBarangResponse> GetAll()
        {
            var models = await dep.GetAll();
            if (models == null)
            {
                return null;
            }
            return models;
        }



        public async Task<NegoBarangResponse> GetPost(NegoBarangRequest req)
        {
            var model = await dep.GetPost(req);

            if (model == null)
            {
                return null;
            }
            return model;

        }

        public async Task<NegoBarangResponse> AddPost(NegoBarangRequest req)
        {
            return await dep.AddPost(req);
           
        }

        public async Task<NegoBarangResponse> DeletePost(NegoBarangRequest req)
        {
           
            return await dep.DeletePost(req);
            
        }

        public async Task<NegoBarangResponse> UpdatePost(NegoBarangRequest req)
        {
            return await dep.UpdatePost(req);
        }
    }
}
