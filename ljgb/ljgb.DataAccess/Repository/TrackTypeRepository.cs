using ljgb.DataAccess.Models;
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
    public class TrackTypeRepository : ITrackType
    {
        ljgbContext db;
        public TrackTypeRepository(ljgbContext _db)
        {
            db = _db;
        }
        public async Task<long> AddPost(TrackType model)
        {
            if (db != null)
            {
                await db.TrackType.AddAsync(model);
                await db.SaveChangesAsync();

                return model.Id;
            }

            return 0;
        }

        public async Task<long> DeletePost(long ID)
        {
            int result = 0;

            if (db != null)
            {
                //Find the warna for specific userprofile
                var model = await db.TrackLevel.FirstOrDefaultAsync(x => x.Id == ID);

                if (model != null)
                {
                    //Delete that warna
                    model.RowStatus = false;
                    db.TrackLevel.Update(model);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<List<TrackTypeViewModel>> GetAll()
        {

            if (db != null)
            {
                return await (from model in db.Barang
                              where model.RowStatus == true
                              select new TrackTypeViewModel
                              {
                                  ID = model.Id,
                                  Name = model.Name,
                                  Created = model.Created,
                                  CreatedBy = model.CreatedBy,
                                  Modified = model.Modified,
                                  ModifiedBy = model.ModifiedBy,
                                  RowStatus = model.RowStatus
                              }).ToListAsync();
            }

            return null;
        }

        public async Task<TrackTypeViewModel> GetPost(long ID)
        {
            if (db != null)
            {
                return await (from model in db.Barang
                              where model.Id == ID && model.RowStatus == true
                              select new TrackTypeViewModel
                              {
                                  ID = model.Id,
                                  Name = model.Name,
                                  Created = model.Created,
                                  CreatedBy = model.CreatedBy,
                                  Modified = model.Modified,
                                  ModifiedBy = model.ModifiedBy,
                                  RowStatus = model.RowStatus
                              }).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<bool> UpdatePost(TrackType model)
        {
            bool result = false;
            if (db != null)
            {
                try
                {
                    //Delete that warna
                    db.TrackType.Update(model);

                    //Commit the transaction
                    await db.SaveChangesAsync();
                    result = true;
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            return result;
        }
    }
}
