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

        public async Task<TransactionResponse> SubmitBuy(int id, int nominal)
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

                CreatedBy = "Admin",
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

        public async Task<TransactionResponse> SubmitSell(int id, int nominal)
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

                CreatedBy = "Admin",
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
    }
}
