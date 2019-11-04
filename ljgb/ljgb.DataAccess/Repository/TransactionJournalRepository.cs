using ljgb.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using ljgb.DataAccess.ViewModel;
using ljgb.DataAccess.Interface;

namespace ljgb.DataAccess.Repository
{
    public class TransactionJournalRepository :ITransactionJournal
    {
        ljgbContext db;
        public TransactionJournalRepository(ljgbContext _db)
        {
            db = _db;
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
