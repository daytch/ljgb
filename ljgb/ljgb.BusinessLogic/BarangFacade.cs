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
        private IKota da_kota;
        private IMerk da_merk;
        private ITypeBarang da_type;
        private IWarna da_warna;
        private IModelBarang da_model;
        private INegoBarang da_nego;
        private string errMerk, errModel, errType, errWarna, errBarang, errOTR, errHargaFinal, errKota = string.Empty;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            da_kota = new KotaRepository(db);
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

        public async Task<BarangResponse> GetBarangDetail(int id)
        {
            BarangResponse resp = new BarangResponse();


            BarangRequest req = new BarangRequest();
            req.ID = id;
            resp.CarDetail = await dep.GetBarangDetail(id);
            resp.RelatedProducts = dep.GetRelatedProducts(id);
            //req.NegoType = "ask";
            resp.SP_GetPhotoAndWarnaByBarangIDS = await dep.GetPhotoAndWarnaByID(req);

            resp.ListModelForDetail = await dep.GetTypeBarangByBarangID(req);
            //resp.SP_GetPhotoAndWarnaByBarangASKS = await dep.GetPhotoAndWarnaByID(req);
            ////req.NegoType = "bid";
            //resp.SP_GetPhotoAndWarnaByBarangIBIDS = await dep.GetPhotoAndWarnaByID(req);

            resp.IsSuccess = true;
            resp.Message = "Success";
            return resp;
        }

        public BarangResponse GetAllAsksById(BarangRequest req)
        {
            BarangResponse resp = new BarangResponse();
            resp.ListAsks = dep.GetAllAsksById(req);

            return resp;
        }
        public BarangResponse GetAllBidsById(BarangRequest req)
        {
            BarangResponse resp = new BarangResponse();
            resp.ListBids = dep.GetAllBidsById(req);

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

        public async Task<BarangResponse> AddPost(BarangRequest request,string email)
        {
            BarangResponse response = new BarangResponse();
            try
            {
                TypeBarangRequest typeRequest = new TypeBarangRequest();
                typeRequest.ID = request.TypeBarangId;
                var getTYpe = da_type.GetPost(typeRequest).Result;
                Barang model = new Barang();
                model.HargaOtr = request.HargaOtr;
                model.Name = getTYpe.Model.Name;
                model.WarnaId = request.WarnaId;
                model.PhotoPath = request.PhotoPath;
                model.TypeBarangId = request.TypeBarangId;
                model.Created = DateTime.Now;
                model.CreatedBy = email;
                model.RowStatus = true;
                model.KotaId = request.KotaID.Value;

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

        public async Task<BarangResponse> DeletePost(long ID, string username)
        {
            BarangResponse response = new BarangResponse();
            try
            {
                long result = 0;
                result = await dep.DeletePost(ID, username);
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
        public async Task<BarangResponse> UpdateImageBarang(BarangRequest request, string username)
        {
            BarangResponse response = new BarangResponse();

            try
            {
                Barang model = new Barang()
                {
                    Id = request.ID
                };
                List<Barang> ListBarang = await dep.GetAllBarangSameTypeAndKota(model);

                foreach (Barang brg in ListBarang)
                {
                    brg.PhotoPath = request.PhotoPath;
                    brg.Modified = DateTime.Now;
                    brg.ModifiedBy = username;
                }

                bool result = await dep.UpdateMany(ListBarang);

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
                log.Error(ex);
                response.Message = ex.ToString();
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<BarangResponse> UpdatePost(BarangRequest request, string username)
        {
            BarangResponse response = new BarangResponse();

            try
            {
                TypeBarangRequest typeRequest = new TypeBarangRequest();
                typeRequest.ID = request.TypeBarangId;
                var getTYpe = da_type.GetPost(typeRequest).Result;
                Barang model = new Barang();
                model.Id = request.ID;
                model.HargaOtr = request.HargaOtr;
                model.Name = getTYpe.Model.Name;
                model.WarnaId = request.WarnaId;
                model.TypeBarangId = request.TypeBarangId;
                model.KotaId = request.KotaID.Value;
                //model.PhotoPath = request.PhotoPath;
                model.Year = request.Year.Value;

                model.Modified = DateTime.Now;
                model.ModifiedBy = username;

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
                log.Error(ex);
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
                log.Error(ex);
                response.Message = ex.ToString();
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<BarangResponse> SubmitUpload(string fileName, string username)
        {
            BarangResponse response = new BarangResponse();
            try
            {
                string folderName = Path.Combine("Resources", "UploadDocs");
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                string sheetName = "Master";
                DataTable dt = new DataTable();

                string dbPath = Path.Combine(folderName, fileName);

                List<Kota> ListKota = await da_kota.GetAll();

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
                            long KotaID = 0;
                            long MerkID = 0;
                            long ModelID = 0;
                            long TypeID = 0;
                            long WarnaID = 0;
                            long BarangID = 0;
                            long NegoBarangID = 0;

                            errMerk = dt.Rows[i].ItemArray.GetValue(1).ToString();
                            errModel = dt.Rows[i].ItemArray.GetValue(2).ToString();
                            errType = dt.Rows[i].ItemArray.GetValue(3).ToString();
                            errWarna = dt.Rows[i].ItemArray.GetValue(4).ToString();
                            errBarang = dt.Rows[i].ItemArray.GetValue(5).ToString();
                            errOTR = dt.Rows[i].ItemArray.GetValue(6).ToString();
                            errHargaFinal = dt.Rows[i].ItemArray.GetValue(8).ToString();

                            #region Insert To Kota 
                            string Kota = dt.Rows[i].ItemArray.GetValue(0).ToString();
                            if (!ListKota.Where(x => x.Name.ToLower() == Kota.ToLower()).Any())
                            {
                                Kota k = new Kota();
                                k.Name = Kota;
                                k.Description = Kota;

                                k.RowStatus = true;
                                k.Created = DateTime.Now;
                                k.CreatedBy = username;
                                KotaID = da_kota.AddPost(k).Result;
                                ListKota.Add(k);
                            }
                            else
                            {
                                KotaID = ListKota.Where(x => x.Name.ToLower() == Kota.ToLower() && x.RowStatus == true).First().Id;
                            }
                            #endregion

                            #region Insert To Merk 
                            string Merk = dt.Rows[i].ItemArray.GetValue(1).ToString();
                            if (!ListMerk.Where(x => x.Name.ToLower() == Merk.ToLower()).Any())
                            {

                                Merk m = new Merk();
                                m.Name = Merk;
                                m.Description = Merk;
                                m.RowStatus = true;
                                m.Created = DateTime.Now;
                                m.CreatedBy = username;
                                MerkID = da_merk.Add(m).Result;
                                ListMerk.Add(m);
                            }
                            else
                            {
                                MerkID = ListMerk.Where(x => x.Name.ToLower() == Merk.ToLower() && x.RowStatus == true).First().Id;
                            }
                            #endregion

                            #region Insert to ModelBarang
                            string Model = dt.Rows[i].ItemArray.GetValue(2).ToString();
                            if (!ListModel.Where(x => x.Name.ToLower() == Model.ToLower()).Any())
                            {
                                ModelBarang mb = new ModelBarang();
                                mb.MerkId = MerkID;
                                mb.Name = Model;
                                mb.Description = Model;
                                mb.RowStatus = true;
                                mb.Created = DateTime.Now;
                                mb.Createdby = username;
                                ModelID = da_model.Add(mb).Result;
                                ListModel.Add(mb);
                            }
                            else
                            {
                                ModelID = ListModel.Where(x => x.Name.ToLower() == Model.ToLower() && x.RowStatus == true).First().Id;
                            }
                            #endregion

                            #region Insert to TypeBarang
                            string Type = dt.Rows[i].ItemArray.GetValue(4).ToString();
                            if (!ListType.Where(x => x.Name.ToLower() == Type.ToLower()).Any())
                            {
                                TypeBarang tb = new TypeBarang();
                                tb.Name = Type;
                                tb.ModelBarangId = ModelID;
                                tb.Description = Type;
                                tb.RowStatus = true;
                                tb.Created = DateTime.Now;
                                tb.CreatedBy = username;
                                TypeID = da_type.Add(tb).Result;
                                ListType.Add(tb);
                            }
                            else
                            {
                                TypeID = ListType.Where(x => x.RowStatus == true && x.Name.ToLower() == Type.ToLower()).First().Id;
                            }
                            #endregion

                            #region Insert to Warna
                            string Warna = dt.Rows[i].ItemArray.GetValue(6).ToString();
                            if (!ListWarna.Where(x => x.Name.ToLower() == Warna.ToLower()).Any())
                            {
                                Warna w = new Warna();
                                w.Name = Warna;
                                w.Description = Warna;
                                w.Sapcode = dt.Rows[i].ItemArray.GetValue(5).ToString();
                                w.RowStatus = true;
                                w.Created = DateTime.Now;
                                w.CreatedBy = username;
                                WarnaID = da_warna.Add(w).Result;
                                ListWarna.Add(w);
                            }
                            else
                            {
                                WarnaID = ListWarna.Where(x => x.RowStatus == true && x.Name.ToLower() == Warna.ToLower()).First().Id;
                            }
                            #endregion

                            string OTRstrRaw = dt.Rows[i].ItemArray.GetValue(8).ToString();
                            string OTRstr = OTRstrRaw.Contains('.') ? OTRstrRaw.Substring(0, OTRstrRaw.LastIndexOf('.')) : OTRstrRaw;

                            string DiscstrRaw = dt.Rows[i].ItemArray.GetValue(9).ToString();
                            string Discstr = DiscstrRaw.Contains('.') ? DiscstrRaw.Substring(0, DiscstrRaw.LastIndexOf('.')) : DiscstrRaw;

                            string FinalRaw = dt.Rows[i].ItemArray.GetValue(10).ToString();
                            string Finalstr = FinalRaw.Contains('.') ? FinalRaw.Substring(0, FinalRaw.LastIndexOf('.')) : FinalRaw;

                            string YearstrRaw = dt.Rows[i].ItemArray.GetValue(7).ToString();
                            string Yearstr = YearstrRaw.Contains('.') ? YearstrRaw.Substring(0, OTRstrRaw.LastIndexOf('.')) : YearstrRaw;

                            long OTR = Convert.ToInt64(OTRstr);
                            long Discount = Convert.ToInt64(Discstr);
                            long HargaFinal = Convert.ToInt64(Finalstr);
                            int Year = Convert.ToInt32(Yearstr);

                            #region Insert to Barang
                            Barang brg = new Barang()
                            {
                                RowStatus = true,
                                Created = DateTime.Now,
                                CreatedBy = username,
                                HargaOtr = OTR,
                                Name = Type,
                                WarnaId = WarnaID,
                                TypeBarangId = TypeID,
                                KotaId = KotaID,
                                Year = Year,
                                KodeType = dt.Rows[i].ItemArray.GetValue(3).ToString()

                            };
                            BarangID = dep.AddPost(brg).Result;
                            #endregion

                            #region Insert to NegoBarang
                            NegoBarang nb = new NegoBarang()
                            {
                                RowStatus = true,
                                BarangId = BarangID,
                                UserProfileId = 3,
                                TypePenawaran = "ask",
                                Harga = HargaFinal,
                                Created = DateTime.Now,
                                CreatedBy = username
                            };
                            NegoBarangID = da_nego.AddPost(nb).Result;
                            #endregion
                        }
                        response.IsSuccess = true;
                        response.Message = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string Ket = string.Format("Merk={0}, Model={1}, Type={2}, Warna={3}, Barang={4}, HargaOTR={5}, Harga Final={6}, Error={7} ",
                    errMerk, errModel, errType, errWarna, errBarang, errOTR, errHargaFinal, ex.ToString());
                response.Message = Ket;// ex.ToString();
                response.IsSuccess = false;
            }
            return response;
        }

        private DataTable WorksheetToDataTable(ExcelWorksheet oSheet)
        {
            int totalRows = oSheet.Dimension.End.Row;
            int totalCols = 11;
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
                log.Error(ex);
                throw ex;
            }
            return dt;
        }

        public async Task<BarangResponse> GetBarangByHomeParameter(BarangRequest request)
        {
            BarangResponse response = new BarangResponse();
            try
            {

                response.sp_GetBarangByHomeParameters = await dep.GetBarangByHomeParameter(request);
                response.SP_GetBarangByHomeParameterCount = await dep.GetBarangByHomeParameterCount(request);
                response.Total = response.SP_GetBarangByHomeParameterCount.total;
                response.IsSuccess = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.ToString();
            }

            return response;
        }


        public async Task<BarangResponse> GetPhotoAndWarnaByID(BarangRequest request)
        {
            BarangResponse response = new BarangResponse();
            try
            {

                //response.sp_GetBarangByHomeParameters = await dep.GetBarangByHomeParameter(request);
                //response.SP_GetBarangByHomeParameterCount = await dep.GetBarangByHomeParameterCount(request);
                //response.Total = response.SP_GetBarangByHomeParameterCount.total;
                response.SP_GetPhotoAndWarnaByBarangIDS = await dep.GetPhotoAndWarnaByID(request);
                response.IsSuccess = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                response.IsSuccess = false;
                response.Message = ex.ToString();
            }

            return response;

        }

        public async Task<BarangResponse> GetHargaOTRTypeBarangID(BarangRequest request)
        {
            BarangResponse response = new BarangResponse();
            try
            {
                Barang brg = await dep.GetHargaOTRTypeBarangID(request.TypeBarangId);
                response.Model = new BarangViewModel()
                {
                    Id = brg.Id,
                    Name = brg.Name,
                    TypeBarangId = brg.TypeBarangId,
                    HargaOtr = brg.HargaOtr
                };
                response.IsSuccess = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                response.IsSuccess = false;
                response.Message = ex.ToString();
            }

            return response;
        }
    }
}

