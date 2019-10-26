using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ljgb.DataAccess.Repository
{
    public class WilayahRepository : IWilayah
    {
        ljgbContext db;
        public WilayahRepository(ljgbContext _db)
        {
            db = _db;
        }
        public async Task<long> AddPost(Wilayah wilayah)
        {
            if (db != null)
            {
                await db.Wilayah.AddAsync(wilayah);
                await db.SaveChangesAsync();

                return wilayah.Id;
            }

            return 0;
        }

        public async Task<long> DeletePost(long wilayahID)
        {
            int result = 0;

            if (db != null)
            {
                //Find the warna for specific userprofile
                var wilayah = await db.Wilayah.FirstOrDefaultAsync(x => x.Id == wilayahID);

                if (wilayah != null)
                {
                    wilayah.RowStatus = true;
                    //Delete that warna
                    db.Wilayah.Update(wilayah);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<List<WilayahViewModel>> GetAllWilayah()
        {

            if (db != null)
            {
                return await (from model in db.Wilayah
                              where model.RowStatus == true
                              select new WilayahViewModel
                              {
                                  ID = model.Id,
                                  Name = model.Name,
                                  Description = model.Description,
                                  Created = model.Created,
                                  CreatedBy = model.CreatedBy,
                                  Modified = model.Modified,
                                  ModifiedBy = model.ModifiedBy,
                                  RowStatus = model.RowStatus
                              }).ToListAsync();
            }

            return null;
        }

        public async Task<WilayahViewModel> GetPost(long postId)
        {
            if (db != null)
            {
                return await (from model in db.Wilayah
                              where model.Id == postId && model.RowStatus == true
                              select new WilayahViewModel
                              {
                                  ID = model.Id,
                                  Name = model.Name,
                                  Description = model.Description,
                                  Created = model.Created,
                                  CreatedBy = model.CreatedBy,
                                  Modified = model.Modified,
                                  ModifiedBy = model.ModifiedBy,
                                  RowStatus = model.RowStatus
                              }).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<bool> UpdatePost(Wilayah wilayah)
        {
            bool result = false;
            if (db != null)
            {
                try
                {
                    //Delete that warna
                    db.Wilayah.Update(wilayah);

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
