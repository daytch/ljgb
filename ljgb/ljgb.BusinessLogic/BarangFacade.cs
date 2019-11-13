﻿using ljgb.Common.Requests;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.BusinessLogic
{
    public class BarangFacade
    {
        #region Important
        private ljgbContext db;
        private IBarang dep;
        private IMerk da_merk;
        private ITypeBarang da_type;
        private IWarna da_warna;
        private IModelBarang da_model;
        private INegoBarang da_nego;

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
            dep = new BarangRepository(db);
            da_merk = new MerkRepository(db);
            da_type = new TypeBarangRepository(db);
            da_warna = new WarnaRepository(db);
            da_model = new ModelBarangRepository(db);
            da_nego = new NegoBarangRepository(db);
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

        public async Task<BarangResponse> SubmitUpload(string fileName)
        {
            BarangResponse response = new BarangResponse();
            try
            {
                string folderName = Path.Combine("Resources", "UploadDocs");
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                string sheetName = "Master";
                DataTable dt = new DataTable();

                string dbPath = Path.Combine(folderName, fileName);

                List<Merk> ListMerk = await da_merk.GetAllMerk();
                List<ModelBarang> ListModel = await da_model.GetAllModel();
                List<TypeBarang> ListType = await da_type.GetAllType();
                List<Warna> ListWarna = await da_warna.GetWarna();
                List<Barang> ListBarang = await dep.GetAllBarang();

                using (ExcelPackage pck = new ExcelPackage())
                {
                    using (FileStream stream = new FileStream(dbPath, FileMode.Open))
                    {
                        pck.Load(stream);
                        ExcelWorksheet oSheet = pck.Workbook.Worksheets[sheetName];
                        dt = WorksheetToDataTable(oSheet);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            long MerkID = 0;
                            long ModelID = 0;
                            long TypeID = 0;
                            long WarnaID = 0;
                            long BarangID = 0;
                            long NegoBarangID = 0;

                            #region Insert To Merk 
                            string Merk = dt.Rows[i].ItemArray.GetValue(1).ToString();
                            if (!ListMerk.Where(x => x.Name == Merk).Any())
                            {
                                Merk m = new Merk();
                                m.Name = Merk;
                                m.Description = Merk;
                                m.RowStatus = true;
                                m.Created = DateTime.Now;
                                m.CreatedBy = "Admin";
                                MerkID = await da_merk.Add(m);
                            }
                            else
                            {
                                MerkID = ListMerk.Where(x => x.Name == Merk && x.RowStatus == true).First().Id;
                            }
                            #endregion

                            #region Insert to ModelBarang
                            string Model = dt.Rows[i].ItemArray.GetValue(2).ToString();
                            if (!ListModel.Where(x => x.Name == Model).Any())
                            {
                                ModelBarang mb = new ModelBarang();
                                mb.MerkId = MerkID;
                                mb.Name = Model;
                                mb.RowStatus = true;
                                mb.Created = DateTime.Now;
                                mb.Createdby = "Admin";
                                ModelID = await da_model.Add(mb);
                            }
                            else
                            {
                                ModelID = ListModel.Where(x => x.Name == Model && x.RowStatus == true).First().Id;
                            }
                            #endregion

                            #region Insert to TypeBarang
                            string Type = dt.Rows[i].ItemArray.GetValue(3).ToString();
                            if (!ListType.Where(x => x.Name == Type).Any())
                            {
                                TypeBarang tb = new TypeBarang();
                                tb.Name = Type;
                                tb.Description = Type;
                                tb.RowStatus = true;
                                tb.Created = DateTime.Now;
                                tb.CreatedBy = "Admin";
                                TypeID = await da_type.Add(tb);
                            }
                            else
                            {
                                TypeID = ListType.Where(x => x.RowStatus == true && x.Name == Type).First().Id;
                            }
                            #endregion

                            #region Insert to Warna
                            string Warna = dt.Rows[i].ItemArray.GetValue(4).ToString();
                            if (!ListWarna.Where(x => x.Name == Type).Any())
                            {
                                Warna w = new Warna();
                                w.Name = Warna;
                                w.Description = Warna;
                                w.RowStatus = true;
                                w.Created = DateTime.Now;
                                w.CreatedBy = "Admin";
                                WarnaID = await da_warna.Add(w);
                            }
                            else
                            {
                                WarnaID = ListWarna.Where(x => x.RowStatus == true && x.Name == Warna).First().Id;
                            }
                            #endregion

                            int OTR = (dt.Rows[i].ItemArray.GetValue(5) != null) ? Convert.ToInt32(dt.Rows[i].ItemArray.GetValue(6)) : 0;
                            int Discount = (dt.Rows[i].ItemArray.GetValue(6) != null) ? Convert.ToInt32(dt.Rows[i].ItemArray.GetValue(7)) : 0;
                            int HargaFinal = (dt.Rows[i].ItemArray.GetValue(7) != null) ? Convert.ToInt32(dt.Rows[i].ItemArray.GetValue(8)) : 0;

                            #region Insert to Barang
                            Barang brg = new Barang()
                            {
                                Created = DateTime.Now,
                                CreatedBy = "Admin",
                                HargaOtr = OTR,
                                Name = Type,
                                WarnaId = WarnaID,
                                TypeBarangId = TypeID
                            };
                            BarangID = await dep.AddPost(brg);
                            #endregion

                            #region Insert to NegoBarang
                            NegoBarang nb = new NegoBarang()
                            {
                                BarangId = BarangID,
                                UserProfileId = 3,
                                TypePenawaran = "ask",
                                Harga = HargaFinal,
                                Created = DateTime.Now,
                                CreatedBy = "Admin"
                            };
                            NegoBarangID = await da_nego.AddPost(nb);
                            #endregion
                        }
                        response.IsSuccess = true;
                        response.Message = "Success";
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

        private DataTable WorksheetToDataTable(ExcelWorksheet oSheet)
        {
            int totalRows = oSheet.Dimension.End.Row;
            int totalCols = 9;
            DataTable dt = new DataTable(oSheet.Name);
            DataRow dr = dt.NewRow();
            try
            {
                for (int i = 1; i <= totalRows; i++)
                {
                    if (oSheet.Cells[i, 1].Value != null)
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
    }
}

