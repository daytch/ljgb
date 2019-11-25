using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.Common.ViewModel;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.BusinessLogic
{
    public class TransactionFacade
    {
        #region Important
        private ljgbContext db;
        private ITransaction dep;
        private INegoBarang INego;
        private IAuthentication IAuth;
        private ITransactionJournal IJournal;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TransactionFacade()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            db = new ljgbContext(optionsBuilder.Options);
            this.IJournal = new TransactionJournalRepository(db);
            this.INego = new NegoBarangRepository(db);
            this.dep = new TransactionRepository(db);
            IAuth = new AuthenticationRepository(db);
        }
        #endregion

        public async Task<TransactionResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            var models = await dep.GetAll(search, order, orderDir, startRec, pageSize, draw);
            if (models == null)
            {
                return null;
            }
            return models;
        }

        public async Task<TransactionResponse> GetPost(TransactionRequest req)
        {
            var model = await dep.GetPost(req);

            if (model == null)
            {
                return null;
            }
            return model;

        }

        public async Task<TransactionResponse> SubmitBuy(int id, int nominal, string username)
        {
            TransactionResponse response = new TransactionResponse();
            NegoBarang nego = new NegoBarang() { BarangId = id, TypePenawaran = "ask", Harga = nominal };
            NegoBarang ResultNego = await INego.GetNegoBarang(nego);
            Transaction tran = new Transaction()
            {
                BuyerId = 4,
                SellerId = ResultNego.UserProfileId,
                NegoBarangId = ResultNego.Id,
                TransactionLevelId = 1,

                CreatedBy = username,
                Created = DateTime.Now,
                RowStatus = true
            };

            long TransID = await dep.SaveTransaction(tran);
            if (TransID < 1)
            {
                response.Message = "Failed When Save Transaction";
                response.IsSuccess = false;
            }
            else
            {
                TransactionJournal journal = new TransactionJournal()
                {
                    TransactionId = tran.Id,
                    BuyerId = tran.BuyerId,
                    SellerId = tran.SellerId,
                    NegoBarangId = tran.NegoBarangId,
                    TransactionLevelId = tran.TransactionLevelId,

                    Created = tran.Created,
                    CreatedBy = tran.CreatedBy,
                    RowStatus = tran.RowStatus,
                };
                long JournalID = await IJournal.SaveTransactionJournal(journal);

                if (JournalID < 1)
                {
                    log.Error("Failed when insert TransactionJournal, TransactionID = " + tran.Id);
                }
                response.Message = "Transaction Success.";
                response.IsSuccess = true;
            }
            return response;
        }

        public async Task<AuthenticationResponse> GetUserProfile(string email)
        {
            AuthenticationResponse resp = new AuthenticationResponse();
            try
            {
                UserProfile userProfile = await IAuth.GetUserProfileByEmail(email);
                resp.Name = userProfile.Nama;
                resp.ID = userProfile.Id;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return resp;
        }

        public async Task<TransactionResponse> SubmitSell(int id, int nominal, string username)
        {
            TransactionResponse response = new TransactionResponse();
            NegoBarang nego = new NegoBarang() { BarangId = id, TypePenawaran = "bid", Harga = nominal };
            NegoBarang ResultNego = await INego.GetNegoBarang(nego);
            Transaction tran = new Transaction()
            {
                BuyerId = ResultNego.UserProfileId,
                SellerId = 4,
                NegoBarangId = ResultNego.Id,
                TransactionLevelId = 1,

                CreatedBy = username,
                Created = DateTime.Now,
                RowStatus = true
            };

            long TransID = await dep.SaveTransaction(tran);
            if (TransID < 1)
            {
                response.Message = "Failed When Save Transaction";
                response.IsSuccess = false;
            }
            else
            {
                TransactionJournal journal = new TransactionJournal()
                {
                    TransactionId = tran.Id,
                    BuyerId = tran.BuyerId,
                    SellerId = tran.SellerId,
                    NegoBarangId = tran.NegoBarangId,
                    TransactionLevelId = tran.TransactionLevelId,

                    Created = tran.Created,
                    CreatedBy = tran.CreatedBy,
                    RowStatus = tran.RowStatus,
                };
                long JournalID = await IJournal.SaveTransactionJournal(journal);

                if (JournalID < 1)
                {
                    log.Error("Failed when insert TransactionJournal, TransactionID = " + tran.Id);
                }
                response.Message = "Transaction Success.";
                response.IsSuccess = true;
            }
            return response;
        }

        public async Task<TransactionResponse> DeletePost(TransactionRequest req)
        {
            return await dep.DeletePost(req);
        }

        public async Task<TransactionResponse> UpdatePost(TransactionRequest req)
        {
            return await dep.UpdatePost(req);
        }

        public async Task<TransactionResponse> CancelTransaction(TransactionRequest req)
        {
            var model = await dep.CancelTransaction(req);

            return model;

        }

        public async Task<TransactionResponse> GetJournalByTransaction(TransactionRequest req)
        {
            return await dep.GetJournalByTransaction(req);
        }

        public async Task<TransactionResponse> GetCurrentTransaction(TransactionRequest req)
        {
            return await dep.GetPost(req);
        }

        public async Task<TransactionResponse> ApproveTransaction(TransactionRequest req)
        {
            TransactionResponse response = new TransactionResponse();
            try
            {
                TransactionResponse getItem = await dep.GetPost(req);
                TransactionLevelFacade transactionLevelFacade = new TransactionLevelFacade();
                TransactionLevelRequest transactionLevelRequest = new TransactionLevelRequest();
                transactionLevelRequest.ID = getItem.ListTransaction.FirstOrDefault().TrasanctionLevel.ID;
                TransactionLevelResponse transactionLevelResponse = new TransactionLevelResponse();
                transactionLevelResponse = await transactionLevelFacade.GetNextLevel(transactionLevelRequest);
                if (transactionLevelResponse.model == null)
                {
                    response.IsSuccess = false;
                    response.Message = transactionLevelResponse.Message;
                    return response;
                }

                req.TrasanctionLevel.ID = transactionLevelResponse.model.ID;

                response = await dep.ApproveTransaction(req);
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.ToString();
            }
            return response;

        }

        public async Task<TransactionResponse> SubmitSell(TransactionRequest req)
        {
            TransactionResponse response = new TransactionResponse();
            Transaction tran = new Transaction()
            {
                BuyerId = req.BuyerID,
                SellerId = req.SellerID,
                NegoBarangId = req.NegoBarangID,
                TransactionLevelId = 1,

                CreatedBy = req.CreatedBy,
                Created = DateTime.Now,
                RowStatus = true
            };
            long TransID = await dep.SaveTransaction(tran);
            if (TransID < 1)
            {
                response.Message = "Failed When Save Transaction";
                response.IsSuccess = false;
            }
            else
            {
                TransactionJournal journal = new TransactionJournal()
                {
                    TransactionId = tran.Id,
                    BuyerId = tran.BuyerId,
                    SellerId = tran.SellerId,
                    NegoBarangId = tran.NegoBarangId,
                    TransactionLevelId = tran.TransactionLevelId,

                    Created = tran.Created,
                    CreatedBy = tran.CreatedBy,
                    RowStatus = tran.RowStatus,
                };
                long JournalID = await IJournal.SaveTransactionJournal(journal);

                if (JournalID < 1)
                {
                    log.Error("Failed when insert TransactionJournal, TransactionID = " + tran.Id);
                }
                response.Message = "Transaction Success.";
                response.IsSuccess = true;
            }
            return response;
        }

        public async Task<TransactionResponse> GetHistory(TransactionRequest req)
        {
            return await IJournal.GetHistory(req.ID);
        }

        public async Task<TransactionResponse> GetListBidAndBuy(TransactionRequest request)
        {
            TransactionResponse response = new TransactionResponse();
            try
            {
                UserProfile userProfile = await dep.GetUserProfile(request.UserName);
                response.ListBidAndBuy = await dep.GetAllBidAndBuyByUserProfileID(userProfile.Id);
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

        public async Task<TransactionResponse> GetAllStatus()
        {
            TransactionResponse response = new TransactionResponse();
            try
            {
                var model = await dep.GetAllStatus();
                response.ListStatus = (from m in model
                                       orderby m.Name ascending
                                       select new TransactionStatusViewModel
                                       {
                                           ID = m.Id,
                                           Name = m.Name,
                                       }).ToList();

                response.IsSuccess = true;
                response.Message = "sucess";
            }
            catch (Exception ex)
            {

                response.Message = ex.ToString();
                response.IsSuccess = false;
            }



            return response;
        }

        public byte[] downloadExcel(TransactionRequest request)
        {

            //List<Model.SPDocument> listSPDocumentModel = new List<Model.SPDocument>();
            try
            {
                //listSPDocumentModel = searchDocument(request, ctx);

                #region populateData
                //List<string> type = listSPDocument.listDocument.Select(x => x.spConfigDocumentTypeName).ToList();
                //List<string> docNo = listSPDocument.listDocument.Select(x => x.docNo).ToList();
                //List<string> npk = listSPDocument.listDocument.Select(x => x.NPK).ToList();
                //List<string> name = listSPDocument.listDocument.Select(x => x.Name).ToList();
                //List<string> position = listSPDocument.listDocument.Select(x => x.position).ToList();
                //List<string> baCode = listSPDocument.listDocument.Select(x => x.branchCode).ToList();
                //List<string> baName = listSPDocument.listDocument.Select(x => x.branchName).ToList();
                //List<decimal> minimumTarget = listSPDocument.listDocument.Select(x => x.minimumProductivity).ToList();
                //List<decimal> actual = listSPDocument.listDocument.Select(x => x.actualProductivity).ToList();
                //List<DateTime> documentDate = listSPDocument.listDocument.Select(x => x.documentDate).ToList();
                //List<DateTime?> printDate = listSPDocument.listDocument.Select(x => x.printDate).ToList();
                //List<DateTime?> goodsIssue = listSPDocument.listDocument.Select(x => x.goodsIssue).ToList();
                //List<DateTime?> goodsReceive = listSPDocument.listDocument.Select(x => x.goodsReceive).ToList();
                //List<DateTime?> goodsSigned = listSPDocument.listDocument.Select(x => x.signed).ToList();
                //List<string> status = listSPDocument.listDocument.Select(x => x.spDocumentAssignmentStatusName).ToList();
                #endregion

                using (ExcelPackage pck = new ExcelPackage())
                {

                    ExcelWorksheet wsDocument = pck.Workbook.Worksheets.Add("TransactionReport");

                    wsDocument.Cells["A1"].Value = "Buyer Name";
                    wsDocument.Cells["B1"].Value = "Buyer Address";
                    wsDocument.Cells["C1"].Value = "Buyer City";
                    wsDocument.Cells["D1"].Value = "Buyer Telp";
                    wsDocument.Cells["E1"].Value = "Buyer Email";
                    wsDocument.Cells["F1"].Value = "Buyer Facebook";
                    wsDocument.Cells["G1"].Value = "Buyer IG";
                    wsDocument.Cells["H1"].Value = "Seller Name";
                    wsDocument.Cells["I1"].Value = "Seller Address";
                    wsDocument.Cells["J1"].Value = "Seller City";
                    wsDocument.Cells["K1"].Value = "Seller Telp";
                    wsDocument.Cells["L1"].Value = "Seller Email";
                    wsDocument.Cells["M1"].Value = "Seller Facebook";
                    wsDocument.Cells["N1"].Value = "Seller IG";
                    wsDocument.Cells["O1"].Value = "Dealer Code";
                    wsDocument.Cells["P1"].Value = "Dealer Address";
                    wsDocument.Cells["Q1"].Value = "Dealer City";
                    wsDocument.Cells["R1"].Value = "Dealer Telp";
                    wsDocument.Cells["S1"].Value = "Dealer Functioner";
                    wsDocument.Cells["T1"].Value = "Brand";
                    wsDocument.Cells["U1"].Value = "Model";
                    wsDocument.Cells["V1"].Value = "Type";
                    wsDocument.Cells["W1"].Value = "Colour";
                    wsDocument.Cells["X1"].Value = "OTR Price";
                    wsDocument.Cells["Y1"].Value = "ASK/BID Price";
                    wsDocument.Cells["Z1"].Value = "ASK/BID";
                    wsDocument.Cells["AA1"].Value = "Transaction Created";
                    wsDocument.Cells["AB1"].Value = "Transaction CreateBy";
                    wsDocument.Cells["AC1"].Value = "Transaction Modified";
                    wsDocument.Cells["AD1"].Value = "Transaction ModifiedBy";
                    wsDocument.Cells["AE1"].Value = "Status";


                    #region Generate SPDocument sheet

                    List<SP_ReportByStatusID> report = dep.GetReportByStatusID(request.TransactionStatusID, request.EndDate).Result;



                    for (int i = 0; i < report.Count; i++)
                    {

                        wsDocument.Cells["A" + (2 + i)].Value = report[i].BuyerNama.ToString();
                        wsDocument.Cells["B" + (2 + i)].Value = (report[i].BuyerAlamat != null) ? report[i].BuyerAlamat.ToString() : "";
                        wsDocument.Cells["C" + (2 + i)].Value = (report[i].BuyerKota != null) ? report[i].BuyerKota.ToString() : "";
                        wsDocument.Cells["D" + (2 + i)].Value = (report[i].BuyerTelp != null) ? report[i].BuyerTelp.ToString() : "";
                        wsDocument.Cells["E" + (2 + i)].Value = (report[i].BuyerEmail != null) ? report[i].BuyerEmail.ToString() : "";
                        wsDocument.Cells["F" + (2 + i)].Value = (report[i].BuyerFacebook != null) ? report[i].BuyerFacebook.ToString() : "";
                        wsDocument.Cells["G" + (2 + i)].Value = (report[i].BuyerIG != null) ? report[i].BuyerIG.ToString() : "";
                        wsDocument.Cells["H" + (2 + i)].Value = (report[i].SellerNama != null) ? report[i].SellerNama.ToString() : "";
                        wsDocument.Cells["I" + (2 + i)].Value = (report[i].SellerAlamat != null) ? report[i].SellerAlamat.ToString() : "";
                        wsDocument.Cells["J" + (2 + i)].Value = (report[i].SellerKota != null) ? report[i].SellerKota.ToString() : "";
                        wsDocument.Cells["K" + (2 + i)].Value = (report[i].SellerTelp != null) ? report[i].SellerTelp.ToString() : "";
                        wsDocument.Cells["L" + (2 + i)].Value = (report[i].SellerEmail != null) ? report[i].SellerEmail.ToString() : "";
                        wsDocument.Cells["M" + (2 + i)].Value = (report[i].SellerFacebook != null) ? report[i].SellerFacebook.ToString() : "";
                        wsDocument.Cells["N" + (2 + i)].Value = (report[i].SellerIG != null) ? report[i].SellerIG.ToString() : "";
                        wsDocument.Cells["O" + (2 + i)].Value = (report[i].SellerDealerKode != null) ? report[i].SellerDealerKode.ToString() : "";
                        wsDocument.Cells["P" + (2 + i)].Value = (report[i].DealerAlamat != null) ? report[i].DealerAlamat.ToString() : "";
                        wsDocument.Cells["Q" + (2 + i)].Value = (report[i].DealerKota != null) ? report[i].DealerKota.ToString() : "";
                        wsDocument.Cells["R" + (2 + i)].Value = (report[i].DealerTelp != null) ? report[i].DealerTelp.ToString() : "";
                        wsDocument.Cells["S" + (2 + i)].Value = (report[i].DealerPejabat != null) ? report[i].DealerPejabat.ToString() : "";
                        wsDocument.Cells["T" + (2 + i)].Value = (report[i].BarangMerk != null) ? report[i].BarangMerk.ToString() : "";
                        wsDocument.Cells["U" + (2 + i)].Value = (report[i].BarangModel != null) ? report[i].BarangModel.ToString() : "";
                        wsDocument.Cells["V" + (2 + i)].Value = (report[i].BarangType != null) ? report[i].BarangType.ToString() : "";
                        wsDocument.Cells["W" + (2 + i)].Value = (report[i].BarangWarna != null) ? report[i].BarangWarna.ToString() : "";
                        wsDocument.Cells["X" + (2 + i)].Value = (report[i].BarangOTR != null) ? report[i].BarangOTR.ToString() : "";
                        wsDocument.Cells["Y" + (2 + i)].Value = (report[i].NegoHarga != null) ? report[i].NegoHarga.ToString() : "";
                        wsDocument.Cells["Z" + (2 + i)].Value = (report[i].NegoType != null) ? report[i].NegoType.ToString() : "";
                        wsDocument.Cells["AA" + (2 + i)].Value = (report[i].TransactionCreated.Date != null) ? report[i].TransactionCreated.Date.ToString() : "";
                        wsDocument.Cells["AB" + (2 + i)].Value = (report[i].TransactionCreatedBy != null) ? report[i].TransactionCreatedBy.ToString() : "";
                        wsDocument.Cells["AC" + (2 + i)].Value = (report[i].TransactionModified != null) ? report[i].TransactionModified.Value.Date.ToString() : "";
                        wsDocument.Cells["AD" + (2 + i)].Value = (report[i].TransactionModifiedBy != null) ? report[i].TransactionModifiedBy.ToString() : "";
                        wsDocument.Cells["AE" + (2 + i)].Value = report[i].TransactionStatus.ToString();
                    }
                    wsDocument.Column(1).AutoFit();
                    wsDocument.Column(2).AutoFit();
                    wsDocument.Column(3).AutoFit();
                    wsDocument.Column(4).AutoFit();
                    wsDocument.Column(5).AutoFit();
                    wsDocument.Column(6).AutoFit();
                    wsDocument.Column(7).AutoFit();
                    wsDocument.Column(8).AutoFit();
                    wsDocument.Column(9).AutoFit();
                    wsDocument.Column(10).AutoFit();
                    wsDocument.Column(11).AutoFit();
                    wsDocument.Column(12).AutoFit();
                    wsDocument.Column(13).AutoFit();
                    wsDocument.Column(14).AutoFit();
                    wsDocument.Column(15).AutoFit();
                    wsDocument.Column(16).AutoFit();
                    wsDocument.Column(17).AutoFit();
                    wsDocument.Column(18).AutoFit();
                    wsDocument.Column(19).AutoFit();
                    wsDocument.Column(20).AutoFit();
                    wsDocument.Column(21).AutoFit();
                    wsDocument.Column(22).AutoFit();
                    wsDocument.Column(23).AutoFit();
                    wsDocument.Column(24).AutoFit();
                    wsDocument.Column(25).AutoFit();
                    wsDocument.Column(26).AutoFit();
                    wsDocument.Column(27).AutoFit();
                    wsDocument.Column(28).AutoFit();
                    wsDocument.Column(29).AutoFit();
                    wsDocument.Column(30).AutoFit();
                    wsDocument.Column(31).AutoFit();

                    wsDocument.Cells["A1:AE1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    wsDocument.Cells["A1:AE1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Aqua);
                    wsDocument.Cells["A1:AE1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsDocument.Cells["A1:AE1"].Style.Font.Bold = true;
                    wsDocument.Cells["A1:AE1"].Style.Font.Size = 14;
                    #endregion

                    string folderName = Path.Combine("Resources", "UploadDocs");
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + "Report.xslx";
                    string phyPath = Path.Combine(folderName, uniqueFileName);

                    FileInfo excelIFIle = new FileInfo(phyPath);
                    pck.SaveAs(excelIFIle);



                    return pck.GetAsByteArray();

                }



            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<TransactionResponse> GetAllBidByUserProfileID(TransactionRequest request)
        {
            TransactionResponse response = new TransactionResponse();
            try
            {
                response.ListSP_GetAllBidByUserProfileID = dep.GetAllBidByUserProfileID(request.UserProfileID).Result;
                response.IsSuccess = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message.ToString();
            }
            return response;
        }

        public async Task<TransactionResponse> GetAllAskByUserProfileID(TransactionRequest request)
        {
            TransactionResponse response = new TransactionResponse();
            try
            {
                response.ListSP_GetAllAskByUserProfileID = dep.GetAllAskByUserProfileID(request.UserProfileID).Result;
                response.IsSuccess = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message.ToString();
            }
            return response;
        }
    }
}
