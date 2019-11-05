using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using ljgb.DataAccess.ViewModel;

namespace ljgb.DataAccess.Repository
{
    public class TrackStatusRepository : ITrackStatus
    {
        //ljgbContext db;
        //public TrackStatusRepository(ljgbContext _db)
        //{
        //    db = _db;
        //}
        //public async Task<long> AddPost(TrackStatus model)
        //{
        //    if (db != null)
        //    {
        //        await db.TrackStatus.AddAsync(model);
        //        await db.SaveChangesAsync();

        //        return model.Id;
        //    }

        //    return 0;
        //}

        //public async Task<long> DeletePost(long ID)
        //{
        //    int result = 0;

        //    if (db != null)
        //    {
        //        //Find the warna for specific userprofile
        //        var model = await db.TrackStatus.FirstOrDefaultAsync(x => x.Id == ID);

        //        if (model != null)
        //        {
        //            //Delete that warna
        //            model.RowStatus = false;
        //            db.TrackStatus.Update(model);

        //            //Commit the transaction
        //            result = await db.SaveChangesAsync();
        //        }
        //        return result;
        //    }

        //    return result;
        //}

        //public async Task<List<TrackStatusViewModel>> GetAll()
        //{

        //    if (db != null)
        //    {
        //        return await (from model in db.TrackStatus
        //                      select new TrackStatusViewModel
        //                      {
        //                          ID = model.Id,
        //                          Name = model.Nama,
        //                          Description = model.Description,
        //                          Created = model.Created,
        //                          CreatedBy = model.CreatedBy,
        //                          Modified = model.Modified,
        //                          ModifiedBy = model.ModifiedBy,
        //                          RowStatus = model.RowStatus
        //                      }).ToListAsync();
        //    }

        //    return null;
        //}

        //public async Task<TrackStatusViewModel> GetPost(long ID)
        //{
        //    if (db != null)
        //    {
        //        return await (from model in db.TrackStatus
        //                      where model.Id == ID
        //                      select new TrackStatusViewModel
        //                      {
        //                          ID = model.Id,
        //                          Name = model.Nama,
        //                          Description = model.Description,
        //                          Created = model.Created,
        //                          CreatedBy = model.CreatedBy,
        //                          Modified = model.Modified,
        //                          ModifiedBy = model.ModifiedBy,
        //                          RowStatus = model.RowStatus
        //                      }).FirstOrDefaultAsync();
        //    }

        //    return null;
        //}

        //public async Task<bool> UpdatePost(TrackStatus model)
        //{
        //    bool result = false;
        //    if (db != null)
        //    {
        //        try
        //        {
        //            //Delete that warna
        //            db.TrackStatus.Update(model);

        //            //Commit the transaction
        //            await db.SaveChangesAsync();
        //            result = true;
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }

        //    }
        //    return result;
        //}
    }
}
