using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.Repository;
using ljgb.DataAccess.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.BusinessLogic
{
    public class ModelBarangFacade
    {
        #region Important
        private ljgbContext db;
        private IModelBarang dep;

        public ModelBarangFacade()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            db = new ljgbContext(optionsBuilder.Options);
            this.dep = new ModelBarangRepository(db);
        }
        #endregion


        public async Task<ModelBarangResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            var models = await dep.GetAll(search, order, orderDir, startRec, pageSize, draw);
            if (models == null)
            {
                return null;
            }
            return models;
        }

        //public async Task<ModelBarangResponse> GetAllWithoutFilter()
        //{
        //    var models = await dep.GetAllWithoutFilter();
        //    if (models == null)
        //    {
        //        return null;
        //    }
        //    return models;
        //}
        

        public async Task<ModelBarangResponse> GetPost(long ID)
        {
            var model = await dep.GetPost(ID);

            if (model == null)
            {
                return null;
            }
            return model;

        }

        public async Task<ModelBarangResponse> AddPost(ModelBarangRequest model)
        {
            return await dep.AddPost(model);
          
        }

        public async Task<ModelBarangResponse> DeletePost(ModelBarangRequest model)
        {
            
            return await dep.DeletePost(model);
           
        }

        public async Task<ModelBarangResponse> UpdatePost(ModelBarangRequest model)
        {
            return await dep.UpdatePost(model);
            
        }

        public async Task<ModelBarangResponse> GetModelWithMerkID(ModelBarangRequest model)
        {
            return await dep.GetModelWithMerkID(model);
        }

        public async Task<ModelBarangResponse> GetModelByKotaIDMerkID(ModelBarangRequest model)
        {
            ModelBarangResponse response = new ModelBarangResponse();
            try
            {
               
                response.ListSP_ModelByKotaIDMerkID = await dep.GetModelByKotaIDMerkID(model);
            }
            catch (Exception ex)
            {

                throw;
            }
         

            return response;
        }
        
    }
}
