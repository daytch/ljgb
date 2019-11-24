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
        private ITransaction iTrans;
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
            this.iTrans = new TransactionRepository(db);
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

        public async Task<NegoBarangResponse> SubmitBid(NegoBarangRequest req)
        {
            req.TypePenawaran = "bid";
            return await dep.AddPost(req);
           
        }


        public async Task<NegoBarangResponse> Submitask(NegoBarangRequest req)
        {
            NegoBarangResponse response = new NegoBarangResponse();
            NegoBarang model = new NegoBarang();
            try
            {
                if (req.ID > 0)
                {
                    model.Id = req.ID;
                    model.UserProfileId = req.UserProfileID;
                    model.BarangId = req.BarangID;
                    model.Harga = req.Harga;
                    model.TypePenawaran = req.TypePenawaran = "ASK";
                    model.Created = DateTime.Now;
                    model.CreatedBy = req.UserName;
                    model.RowStatus = true;
                    if (await dep.UpdatePost(model) > 0 )
                    {
                        response.IsSuccess = true;
                        response.Message = "Update Success";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Update Failed";
                    }

                }
                else
                {
                    model.UserProfileId = req.UserProfileID;
                    model.BarangId = req.BarangID;
                    model.TypePenawaran = req.TypePenawaran = "ASK";
                    model.Created = DateTime.Now;
                    model.CreatedBy = req.UserName;
                    model.RowStatus = true;
                    model.Harga = req.Harga;
                    if (await dep.AddPost(model) > 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Data Already Saved";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Save Failed";
                    }
                }
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message.ToString();
            }

            //return await dep.AddPost(req);
            return response;

        }

        public async Task<NegoBarangResponse> DeletePost(NegoBarangRequest req)
        {
            NegoBarangResponse response = new NegoBarangResponse();
            try
            {

                if (await dep.DeletePost(req.ID))
                {
                    response.IsSuccess = true;
                    response.Message = "Data Deleted";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Delete data Failed";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = true;
                response.Message = ex.ToString();
            }
            return response;
            
        }

        public async Task<NegoBarangResponse> UpdatePost(NegoBarangRequest req)
        {
            return await dep.UpdatePost(req);
        }

        public async Task<NegoBarangResponse> GetAllASK(string search, string order, string orderDir, int startRec, int pageSize, int draw, string userName)
        {
            NegoBarangResponse resp = new NegoBarangResponse();
            UserProfile usrProfile = iTrans.GetUserProfile(userName).Result;
            UserDetail usrDetail = iTrans.GetUserDetail(usrProfile.Id).Result;
            if (usrDetail != null)
            {
                if (usrDetail.Description.ToLower() == "admin")
                {
                    resp = await dep.GetAllASK(search, order, orderDir, startRec, pageSize, draw);
                }
                else
                {
                    resp = await dep.GetAllASK(search, order, orderDir, startRec, pageSize, draw, usrProfile.Id);
                }
                resp.IsSuccess = true;
                resp.Message = "Success";
            }
            else if (usrProfile != null)
            {

                resp = await dep.GetAllASK(search, order, orderDir, startRec, pageSize, draw, usrProfile.Id);
                resp.IsSuccess = true;
                resp.Message = "Success";

            }
            else
            {
                resp.IsSuccess = false;
                resp.Message = "Expired Token !";
            }
           

            return resp;
        }

        public async Task<NegoBarangResponse> GetAllBID(string search, string order, string orderDir, int startRec, int pageSize, int draw, string userName)
        {
            NegoBarangResponse resp = new NegoBarangResponse();
            UserProfile usrProfile = iTrans.GetUserProfile(userName).Result;
            UserDetail usrDetail = iTrans.GetUserDetail(usrProfile.Id).Result;
            if (usrDetail != null)
            {
                if (usrDetail.Description.ToLower() == "admin")
                {
                    resp = await dep.GetAllBID(search, order, orderDir, startRec, pageSize, draw);
                }
                else
                {
                    resp = await dep.GetAllBID(search, order, orderDir, startRec, pageSize, draw, usrProfile.KotaId);
                }
                resp.IsSuccess = true;
                resp.Message = "Success";
            }
            else if (usrProfile != null)
            {

                resp = await dep.GetAllBID(search, order, orderDir, startRec, pageSize, draw, usrProfile.KotaId);
                resp.IsSuccess = true;
                resp.Message = "Success";

            }
            else
            {
                resp.IsSuccess = false;
                resp.Message = "Expired Token !";
            }


            return resp;
            //NegoBarangResponse response = new NegoBarangResponse();
            //var models = await dep.GetAllBID(search, order, orderDir, startRec, pageSize, draw);
            //if (models == null)
            //{
            //    return null;
            //}
            //return models;

        }
    }
}
