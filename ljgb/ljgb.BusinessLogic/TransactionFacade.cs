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
            this.INego = new NegoBarangRepository(db);
            this.dep = new TransactionRepository(db);
        }
        #endregion

        public async Task<TransactionResponse> GetAll()
        {
            var models = await dep.GetAll();
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

            return await dep.AddPost(tran);
        }

        public async Task<TransactionResponse> SubmitSell(int id, int nominal)
        {
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

            return await dep.AddPost(tran);
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
                if (transactionLevelResponse == null)
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
    }
}
