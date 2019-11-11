using ljgb.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using ljgb.DataAccess.ViewModel;
using ljgb.DataAccess.Interface;
using ljgb.Common.Responses;
using ljgb.Common.ViewModel;

namespace ljgb.DataAccess.Repository
{
    public class TransactionJournalRepository :ITransactionJournal
    {
        ljgbContext db;
        public TransactionJournalRepository(ljgbContext _db)
        {
            db = _db;
        }

        public async Task<TransactionResponse> GetHistory(long ID)
        {
            TransactionResponse response = new TransactionResponse();
            try
            {
                response.ListHistory = await (from history in db.TransactionJournal
                                              join level in db.TransactionLevel
                                              on history.TransactionLevelId equals level.Id
                                              join step in db.TransactionStep
                                              on level.TransactionStepId equals step.Id
                                              join status in db.TransactionStatus
                                              on level.TransactionStepId equals status.Id
                                              where history.RowStatus == true &&
                                              level.RowStatus == true &&
                                              step.RowStatus == true &&
                                              step.RowStatus == true &&
                                              history.TransactionId == ID
                                              select new TransactionHistoryViewModel
                                              {
                                                  TransactionStatus = status.Name,
                                                  TransactionStep = step.Name,
                                                  Created = history.Created
                                              }).ToListAsync();
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.ToString();
            }

            return response;
        }

        public async Task<long> SaveTransactionJournal(TransactionJournal model)
        {
            if (db != null)
            {
                await db.TransactionJournal.AddAsync(model);
                await db.SaveChangesAsync();

                return model.Id;
            }

            return 0;
        }


    }
}
