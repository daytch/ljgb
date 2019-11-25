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
            MerkResponse response = new MerkResponse();
            try
            {

                if (await dep.GetMerkByName(model.Name) != null)
                {
                    response.Message = "Data is Duplicate with Existing Data";
                    response.IsSuccess = false;
                }
                else
                {
                    response = await dep.AddPost(model);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.ToString();
                response.IsSuccess = false;
            }

          return response;
            
        }

        public async Task<MerkResponse> DeletePost(long ID, string username)
        {

            return await dep.DeletePost(ID, username);
           
        }

        public async Task<MerkResponse> UpdatePost(MerkRequest model)
        {
            MerkResponse response = new MerkResponse();
            try
            {
                Merk item = await dep.GetMerkByName(model.Name);
                if (item != null)
                {
                    if (item.Id == model.ID)
                    {
                        response = await dep.UpdatePost(model);
                    }
                    else
                    {
                        response.Message = "Data is Duplicate with Existing Data";
                        response.IsSuccess = false;
                    }
                    
                }
                else
                {
                    response = await dep.UpdatePost(model);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.ToString();
                response.IsSuccess = false;
            }
            return response;

           
        }
        public MerkResponse GetMerkByKotaID(long KotaID)
        {
            MerkResponse response = new MerkResponse();
            try
            {
                response.ListSP_MerkByKotaID = dep.GetMerkByKotaID(KotaID);
                response.IsSuccess = true;
                response.Message = "Success Get Merk";
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = "Something Error with System";
            }
           

            return response;
        }
    }
}
