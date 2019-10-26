using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.Common.ViewModel;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ljgb.DataAccess.Repository
{
    public class TransactionLevelRepository : ITransactionLevel
    {
        ljgbContext db;
        public TransactionLevelRepository(ljgbContext _db)
        {
            db = _db;
        }
        public async Task<TransactionLevelResponse> GetAll()
        {
            TransactionLevelResponse response = new TransactionLevelResponse();
            if (db != null)
            {
                try
                {
                    response.Message = "Success";
                    response.ListTransactionLevel = await (from level in db.TransactionLevel
                                                           join status in db.TransactionStatus
                                                           on level.TransactionStatusId equals status.Id
                                                           join step in db.TransactionStep
                                                           on level.TransactionStepId equals step.Id
                                                           where level.RowStatus == true && status.RowStatus == true
                                                           && step.RowStatus == true
                                                           select new TransactionLevelViewModel
                                                           {
                                                               ID = level.Id,
                                                               Status = new TransactionStatusViewModel
                                                               {
                                                                   ID = status.Id,
                                                                   Name = status.Name,
                                                                   Description = status.Description,
                                                                   Created = status.Created,
                                                                   Createdby = status.CreatedBy,
                                                                   Modified = status.Modified,
                                                                   ModifiedBy = status.ModifiedBy,
                                                                   RowStatus = status.RowStatus
                                                               },
                                                               Name = level.Name,
                                                               Step = new TransactionStepViewModel
                                                               {
                                                                   ID = step.Id,
                                                                   Name = step.Name,
                                                                   Created = step.Created,
                                                                   CreatedBy = step.CreatedBy,
                                                                   Modified = step.Modified,
                                                                   ModifiedBy = step.ModifiedBy,
                                                                   RowStatus = step.RowStatus

                                                               },
                                                               Sequence = level.Sequence,
                                                               Created = level.Created,
                                                               CreatedBy = level.CreatedBy,
                                                               Description = level.Description,
                                                               Modified = level.Modified,
                                                               ModifiedBy = level.ModifiedBy,
                                                               RowStatus = level.RowStatus
                                                           }
                                                     ).ToListAsync();
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }
               
                     
            }
            else
            {

                response.Message = "Database Context is null";
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<TransactionLevelResponse> GetCurrentLevel(TransactionLevelRequest request)
        {

            TransactionLevelResponse response = new TransactionLevelResponse();
            try
            {
                if (db != null)
                {
                    response.Message = "Success";
                    response.model = await (from level in db.TransactionLevel
                                            join status in db.TransactionStatus
                                            on level.TransactionStatusId equals status.Id
                                            join step in db.TransactionStep
                                            on level.TransactionStepId equals step.Id
                                            where level.RowStatus == true && status.RowStatus == true
                                            && step.RowStatus == true && request.ID == level.Id
                                            select new TransactionLevelViewModel
                                            {
                                                ID = level.Id,
                                                Status = new TransactionStatusViewModel
                                                {
                                                    ID = status.Id,
                                                    Name = status.Name,
                                                    Description = status.Description,
                                                    Created = status.Created,
                                                    Createdby = status.CreatedBy,
                                                    Modified = status.Modified,
                                                    ModifiedBy = status.ModifiedBy,
                                                    RowStatus = status.RowStatus
                                                },
                                                Name = level.Name,
                                                Step = new TransactionStepViewModel
                                                {
                                                    ID = step.Id,
                                                    Name = step.Name,
                                                    Created = step.Created,
                                                    CreatedBy = step.CreatedBy,
                                                    Modified = step.Modified,
                                                    ModifiedBy = step.ModifiedBy,
                                                    RowStatus = step.RowStatus

                                                },
                                                Sequence = level.Sequence,
                                                Created = level.Created,
                                                CreatedBy = level.CreatedBy,
                                                Description = level.Description,
                                                Modified = level.Modified,
                                                ModifiedBy = level.ModifiedBy,
                                                RowStatus = level.RowStatus
                                            }
                                                     ).FirstOrDefaultAsync();

                }
                else
                {

                    response.Message = "Database Context is null";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

                response.Message = ex.ToString(); ;
                response.IsSuccess = false;
            }
            
            return response;
        }

        public async Task<TransactionLevelResponse> GetNextLevel(TransactionLevelRequest request)
        {
            TransactionLevelResponse response = new TransactionLevelResponse();
            try
            {
                if (db != null)
                {
                    response.Message = "Success";
                    var query =  (from level in db.TransactionLevel
                                       where level.Id == request.ID && level.RowStatus == true
                                       select level);
                    response.model = await (from level in db.TransactionLevel
                                            join q in query
                                            on level.Name equals q.Name
                                            join status in db.TransactionStatus
                                            on level.TransactionStatusId equals status.Id
                                            join step in db.TransactionStep
                                            on level.TransactionStepId equals step.Id
                                            where level.RowStatus == true && status.RowStatus == true
                                            && step.RowStatus == true && (q.Sequence+1) == level.Sequence
                                            select new TransactionLevelViewModel
                                            {
                                                ID = level.Id,
                                                Status = new TransactionStatusViewModel
                                                {
                                                    ID = status.Id,
                                                    Name = status.Name,
                                                    Description = status.Description,
                                                    Created = status.Created,
                                                    Createdby = status.CreatedBy,
                                                    Modified = status.Modified,
                                                    ModifiedBy = status.ModifiedBy,
                                                    RowStatus = status.RowStatus
                                                },
                                                Name = level.Name,
                                                Step = new TransactionStepViewModel
                                                {
                                                    ID = step.Id,
                                                    Name = step.Name,
                                                    Created = step.Created,
                                                    CreatedBy = step.CreatedBy,
                                                    Modified = step.Modified,
                                                    ModifiedBy = step.ModifiedBy,
                                                    RowStatus = step.RowStatus

                                                },
                                                Sequence = level.Sequence,
                                                Created = level.Created,
                                                CreatedBy = level.CreatedBy,
                                                Description = level.Description,
                                                Modified = level.Modified,
                                                ModifiedBy = level.ModifiedBy,
                                                RowStatus = level.RowStatus
                                            }).FirstOrDefaultAsync();

                }
                else
                {

                    response.Message = "Database Context is null";
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
