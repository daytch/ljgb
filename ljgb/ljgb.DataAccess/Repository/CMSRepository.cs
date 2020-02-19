using ljgb.Common.ViewModel;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ljgb.Common.Responses;
using ljgb.Common.Requests;
using System.Data.SqlClient;
using System.Transactions;

namespace ljgb.DataAccess.Repository
{
    public class CMSRepository : ICMS
    {
        ljgbContext db;
        public CMSRepository(ljgbContext _db)
        {
            db = _db;
        }

        public Task<List<CmsmasterData>> GetAll()
        {
            try
            {
                return db.CmsmasterData.Where(x => x.RowStatus == true).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Task<List<SP_CMSMaster>> GetAllByCategory(string Category)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SP_CMSMaster>> GetAllCMSMaster()
        {
            if (db != null)
            {
                try
                {
                    return db.Set<SP_CMSMaster>().FromSql("EXEC sp_GetCMSMasterData").AsNoTracking().ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }

        public async Task<long> SubmitCMS(CMSRequest model)
        {
            if (db != null)
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        foreach (var item in model.List_CMSMaster)
                        {
                            CmsmasterData data = db.CmsmasterData.Where(x => x.RowStatus == true && x.Description == item.Description).First();
                            data.Value = item.Value;
                            await db.SaveChangesAsync();
                        }

                       
                    }
                    catch (Exception ex)
                    {
                        scope.Dispose();
                        
                        throw ex;
                    }
                    scope.Complete();
                }
               

                return 1;
            }

            return 0;
        }
    }
}
