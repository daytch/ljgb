using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.Common.ViewModel;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
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
                model.PhotoPath = request.PhotoPath;
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

        public async Task<BarangResponse> SubmitUpload(string fileName)
        {
            BarangResponse response = new BarangResponse();
            try
            {
                string folderName = Path.Combine("Resources", "UploadDocs");
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                string sheetName = "Sheet1";
                DataTable dt = new DataTable();

                string dbPath = Path.Combine(folderName, fileName);
                using (ExcelPackage pck = new ExcelPackage())
                {
                    using (FileStream stream = new FileStream(dbPath, FileMode.Open))
                    {
                        pck.Load(stream);
                        ExcelWorksheet oSheet = pck.Workbook.Worksheets[sheetName];
                        dt = WorkSheetToDatatable(oSheet);
                    }
                }
            }
            catch (Exception ex)
            {

                response.Message = ex.ToString();
                response.IsSuccess = false;
            }
            return response;
        }

        private DataTable WorkSheetToDatatable(ExcelWorksheet oSheet)
        {
            int totalRows = oSheet.Dimension.End.Row;
            int totalCols = 8;

            DataTable dt = new DataTable(oSheet.Name);
            DataRow dr = dt.NewRow();
            try
            {
                for (int i = 1; i < totalRows; i++)
                {
                    if (oSheet.Cells[1, i].Value != null)
                    {
                        if (i > 1) dr = dt.Rows.Add();
                        for (int j = 1; j <= totalCols; j++)
                        {
                            if (i == 1)
                                dt.Columns.Add(oSheet.Cells[i, j].Value.ToString());
                            else
                                dr[j - 1] = (oSheet.Cells[i, j].Value == null) ? "" : oSheet.Cells[i, j].Value.ToString();
                        }
                    }
                    else
                    {
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public async Task<BarangResponse> GetBarangByHomeParameter(BarangRequest request)
        {
            BarangResponse response = new BarangResponse();

            return response;
        }
    }
}

