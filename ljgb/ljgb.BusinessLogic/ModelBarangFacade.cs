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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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

        public async Task<ModelBarangResponse> GetAllCategory(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            var models = await dep.GetAllCategory(search, order, orderDir, startRec, pageSize, draw);
            if (models == null)
            {
                return null;
            }
            return models;
        }

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
            ModelBarangResponse response = new ModelBarangResponse();
            try
            {
                ModelBarang request = new ModelBarang();
                request.MerkId = model.MerkID;
                request.Name = model.Name;
                ModelBarang result = await dep.GetModelWithMerkIDModelName(request);
                if (result !=null)
                {
                    response.IsSuccess = false;
                    response.Message = "Data Duplicate with Existing";
                }
                else
                {
                    response = await dep.AddPost(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                response.Message = ex.ToString();
                response.IsSuccess = false;
            }
            return response;
          
        }

        public async Task<ModelBarangResponse> DeletePost(ModelBarangRequest model)
        {
            ModelBarangResponse resp = new ModelBarangResponse();
            long result = 0;
            result = await dep.DeletePost(model.ID, model.UserName);
            if (result == 0)
            {
                resp.IsSuccess = false;
                resp.Message = "Failed when delete Model Bareng";
            }
            else
            {
                resp.IsSuccess = true;
                resp.Message = "Success Delete Model Barang";
            }
            return resp;
        }

        public async Task<ModelBarangResponse> UpdatePost(ModelBarangRequest model)
        {
            ModelBarangResponse response = new ModelBarangResponse();
            try
            {
                ModelBarang request = new ModelBarang();
                request.MerkId = model.MerkID;
                request.Name = model.Name;
                ModelBarang result = await dep.GetModelWithMerkIDModelName(request);
                if (result != null )
                {
                    if (model.ID != result.Id)
                    {
                        response.IsSuccess = false;
                        response.Message = "Data Duplicate with Existing";
                    }
                    else
                    {
                        response.IsSuccess = true;
                        response.Message = "Update Success";
                        response = await dep.UpdatePost(model);
                    }
                }
                else
                {
                    response.IsSuccess = true;
                    response.Message = "Update Success";
                    response = await dep.UpdatePost(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                response.Message = ex.ToString();
                response.IsSuccess = false;
            }
            return response;
            
        }

        public async Task<ModelBarangResponse> UpdateCategory(ModelBarangRequest model)
        {
            ModelBarangResponse response = new ModelBarangResponse();
            try
            {
                ModelBarang request = new ModelBarang();
                request.Id = model.ID;

                ModelBarang result = await dep.GetModelWithMerkIDModelName(request);
                model.ID = result.Id;
                model.MerkID = result.MerkId;
                model.Name = result.Name;
                model.Description = result.Description;

                if (result != null)
                {
                    if (model.ID != result.Id)
                    {
                        response.IsSuccess = false;
                        response.Message = "Data Does not exist.";
                    }
                    else
                    {
                        response.IsSuccess = true;
                        response.Message = "Update Success";
                        response = await dep.UpdatePost(model);
                    }
                }
                else
                {
                    response.IsSuccess = true;
                    response.Message = "Update Success";
                    response = await dep.UpdatePost(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                response.Message = ex.ToString();
                response.IsSuccess = false;
            }
            return response;

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
                response.Message = "Success";
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {

                response.Message = ex.ToString(); ;
                response.IsSuccess = false;
            }
         

            return response;
        }
        
    }
}
