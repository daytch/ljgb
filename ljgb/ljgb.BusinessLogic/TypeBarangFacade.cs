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
    public class TypeBarangFacade
    {
        #region Important
        private ljgbContext db;
        private ITypeBarang dep;

        public TypeBarangFacade()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            db = new ljgbContext(optionsBuilder.Options);
            this.dep = new TypeBarangRepository(db);
        }
        #endregion


        public async Task<TypeBarangResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            return await dep.GetAll(search, order, orderDir, startRec, pageSize, draw);
    
        }

        public async Task<TypeBarangResponse> GetAllWithModelID(TypeBarangRequest request)
        {
            return await dep.GetAllWithModelID(request);
        }


        public async Task<TypeBarangResponse> GetPost(TypeBarangRequest request)
        {
            return await dep.GetPost(request);

        }

        //public async Task<TypeBarangResponse> GetTypeByName(TypeBarangRequest request)
        //{
        //    TypeBarangResponse response = new TypeBarangResponse();
            
           

        //    return response;
        //}

        public async Task<TypeBarangResponse> AddPost(TypeBarangRequest request)
        {
            TypeBarangResponse response = new TypeBarangResponse();
            try
            {
                TypeBarang req = new TypeBarang();
                req.Name = request.Name;
                TypeBarang model = await dep.GetTypeByName(req);
                
                if (model!= null)
                {
                    response.IsSuccess = false;
                    response.Message = "Data Duplicate with Existing Data";
                }
                else{
                   
                    req.ModelBarangId = request.ModelBarangID;
                    req.Name = request.Name;
                    req.Description = request.Description;
                    req.Created = DateTime.Now;
                    req.CreatedBy = request.UserName;
                    req.RowStatus = true;
                    response =  await dep.AddPost(req);
                }
            }
            catch (Exception ex)
            {

                response.Message = ex.Message.ToString();
                response.IsSuccess = false;
            }
            return response;


        }

        public async Task<TypeBarangResponse> DeletePost(TypeBarangRequest request)
        {
           
            return await dep.DeletePost(request);
       
        }

        public async Task<TypeBarangResponse> UpdatePost(TypeBarangRequest request)
        {
            TypeBarangResponse response = new TypeBarangResponse();
            try
            {
                TypeBarang req = new TypeBarang();
                req.Name = request.Name;
                TypeBarang model = await dep.GetTypeByName(req);

                if (model != null)
                {
                    if (model.Id != request.ID)
                    {
                        response.IsSuccess = false;
                        response.Message = "Data Duplicate with Existing Data";
                    }
                    else
                    {

                        req.ModelBarangId = request.ModelBarangID;
                        req.Name = request.Name;
                        req.Description = request.Description;
                        req.Created = DateTime.Now;
                        req.CreatedBy = request.UserName;
                        req.RowStatus = true;
                        response = await dep.UpdatePost(request);
                    }
                }
                else
                {

                    req.ModelBarangId = request.ModelBarangID;
                    req.Name = request.Name;
                    req.Description = request.Description;
                    req.Created = DateTime.Now;
                    req.CreatedBy = request.UserName;
                    req.RowStatus = true;
                    response = await dep.UpdatePost(request);
                }
            }
            catch (Exception ex)
            {

                response.Message = ex.Message.ToString();
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<TypeBarangResponse> GetTypeByKotaIDMerkIDModelID (TypeBarangRequest request)
        {
            TypeBarangResponse response = new TypeBarangResponse();
            try
            {

                response.ListSP__TypeByKotaIDMerkIDModelID = await dep.GetTypeByKotaIDMerkIDModelID(request);
                response.Message = "Success";
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = "Failed";
            }

            return response;
        }
    }
}
