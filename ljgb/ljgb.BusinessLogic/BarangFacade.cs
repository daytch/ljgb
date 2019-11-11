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
    public class BarangFacade
    {
        #region Important
        private ljgbContext db;
        private IBarang dep;

        public BarangFacade()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            db = new ljgbContext(optionsBuilder.Options);
            this.dep = new BarangRepository(db);
        }
        #endregion


        public async Task<BarangResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            
            return await dep.GetAll(search, order, orderDir, startRec, pageSize, draw);
          
        }

        public BarangResponse GetAllForHomePage(string city)
        {
            BarangResponse resp = new BarangResponse();
            resp.HighestBids = dep.GetHighestBid(city, 5);
            resp.LowestAsks = dep.GetLowestAsk(city, 5);
            resp.ListNormal = dep.GetListNormal(city, 10);
            return resp;
        }

        public async Task<Position> GetAskPosition(int ID, int Nominal)
        {
            Position pos = await dep.GetAskPosition(ID, Nominal);
            if (pos == null)
            {
                pos = new Position()
                {
                    ID = Convert.ToInt64(ID),
                    price_rank = 1
                };
            }
            return pos;
        }

        public async Task<Position> GetBidPosition(int ID, int Nominal)
        {
            Position pos = await dep.GetBidPosition(ID, Nominal);
            if (pos == null)
            {
                pos = new Position()
                {
                    ID = Convert.ToInt64(ID),
                    price_rank = 1
                };
            }

            return pos;
        }

        public async Task<BarangResponse> GetBarangDetail(int ID)
        {
            BarangResponse resp = new BarangResponse();
            resp.CarDetail = await dep.GetBarangDetail(ID);
            resp.RelatedProducts = dep.GetRelatedProducts(ID);

            return resp;
        }

        public BarangResponse GetAllAsksById(BarangRequest req)
        {
            BarangResponse resp = new BarangResponse();
            resp.ListAsks = dep.GetAllAsksById(req);

            return resp;
        }

        public async Task<BarangViewModel> GetPost(long ID)
        {
            var model = await dep.GetPost(ID);

            if (model == null)
            {
                return null;
            }
            return model;

        }

        public async Task<BarangResponse> AddPost(BarangRequest request)
        {
            BarangResponse response = new BarangResponse();
            try
            {
                Barang model = new Barang();
                model.HargaOtr = request.HargaOtr;
                model.Name = request.Name;
                model.WarnaId = request.WarnaId;
                model.TypeBarangId = request.TypeBarangId;
                model.Created = DateTime.Now;
                model.CreatedBy = "xsivicto1905";
                model.RowStatus = true;


                var postId = await dep.AddPost(model);
                if (postId > 0)
                {
                    response.Message = "Data Already Saved";
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = "Save Failed";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

                response.Message = ex.ToString();
                response.IsSuccess = false;
            }
            return response;
           
        }

        public async Task<BarangResponse> DeletePost(long ID)
        {
            BarangResponse response = new BarangResponse();
            try
            {
                long result = 0;
                result = await dep.DeletePost(ID);
                if (result == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Delete Failed";
                }
                else
                {
                    response.IsSuccess = true;
                    response.Message = "Data has been deleted";
                }
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.ToString();
            }
            
            return response;
        }

        public async Task<BarangResponse> UpdatePost(BarangRequest request)
        {
            BarangResponse response = new BarangResponse();

            try
            {
                Barang model = new Barang();
                model.Id = request.ID;
                model.HargaOtr = request.HargaOtr;
                model.Name = request.Name;
                model.WarnaId = request.WarnaId;
                model.TypeBarangId = request.TypeBarangId;
                model.Modified = DateTime.Now;
                model.ModifiedBy = "xsivicto1905";
                bool result = await dep.UpdatePost(model);

                if (result)
                {
                    response.Message = "Data Already Saved";
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = "Update Failed";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

                response.Message = ex.ToString();
                response.IsSuccess = false;
            }


            return response;
        }

        public async Task<BarangResponse> GetHargaOTR(BarangRequest request)
        {
            BarangResponse response = new BarangResponse();
            try
            {
                Barang model = new Barang();
                model.WarnaId = request.WarnaId;
                model.TypeBarangId = request.TypeBarangId;

                var result = await dep.GetHargaOTR(model);

                if (result != null)
                {
                    response.Model.Id = result.Id;
                    response.Model.HargaOtr = result.HargaOtr;
                    response.IsSuccess = true;
                    response.Message = "Load Harga OTR Success";
                }
                else
                {
                    response.Message = "Data dengan Warna dan Type tersebut Tidak ada";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

                response.Message = ex.ToString();
                response.IsSuccess = false;
            }
            return response;
        }
    }
}

