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
using System.Text;
using System.Threading.Tasks;

namespace ljgb.BusinessLogic
{
    public class MerkFacade
    {
        #region Important
        private ljgbContext db;
        private IMerk dep;

        public MerkFacade()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            db = new ljgbContext(optionsBuilder.Options);
            this.dep = new MerkRepository(db);
        }
        #endregion


        public async Task<MerkResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            var models = await dep.GetAll(search, order, orderDir, startRec, pageSize, draw);
            if (models == null)
            {
                return null;
            }
            return models;
        }

        public async Task<MerkResponse> GetAllWithoutFilter()
        {
            return await dep.GetAllWithoutFilter();
        }

        public async Task<MerkResponse> GetPost(long ID)
        {
            var model = await dep.GetPost(ID);

            if (model == null)
            {
                return null;
            }
            return model;

        }

        public async Task<MerkResponse> AddPost(MerkRequest model)
        {
          return await dep.AddPost(model);
            
        }

        public async Task<MerkResponse> DeletePost(long ID)
        {

            return await dep.DeletePost(ID);
           
        }

        public async Task<MerkResponse> UpdatePost(MerkRequest model)
        {
           return await dep.UpdatePost(model);

           
        }
        public MerkResponse GetMerkByKotaID(long KotaID)
        {
            MerkResponse response = new MerkResponse();
            response.ListSP_MerkByKotaID = dep.GetMerkByKotaID(KotaID);

            return response;
        }
    }
}
