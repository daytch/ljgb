using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.Common.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ljgb.DataAccess.Repository
{
    public class WarnaRepository : IWarna
    {
        ljgbContext db;
        public WarnaRepository(ljgbContext _db)
        {
            db = _db;
        }

        public async Task<List<Warna>> GetWarna()
        {
            if (db != null)
            {
                return await db.Warna.Where(x=>x.RowStatus == true).ToListAsync();
            }

            return null;
        }

        public async Task<long> Add(Warna model)
        {
            if (db != null)
            {
                await db.Warna.AddAsync(model);
                await db.SaveChangesAsync();

                return model.Id;
            }

            return 0;
        }

        public async Task<List<WarnaViewModel>> GetPosts()
        {
            if (db != null)
            {
                return await (from w in db.Warna
                              where w.RowStatus == true
                              select new WarnaViewModel
                              {
                                  Id = w.Id,
                                  Name = w.Name,
                                  Description = w.Description,
                                  Created = w.Created,
                                  CreatedBy = w.CreatedBy,
                                  RowStatus = w.RowStatus
                              }).ToListAsync();
            }

            return null;
        }
        public async Task<List<WarnaViewModel>> GetAllWithoutFilter()
        {
            if (db != null)
            {
                return await (from w in db.Warna
                              where w.RowStatus == true
                              select new WarnaViewModel
                              {
                                  Id = w.Id,
                                  Name = w.Name,
                                  Description = w.Description,
                                  Created = w.Created,
                                  CreatedBy = w.CreatedBy,
                                  RowStatus = w.RowStatus
                              }).ToListAsync();
            }

            return null;
        }
        
        public async Task<WarnaViewModel> GetPost(long postId)
        {
            if (db != null)
            {
                return await (from w in db.Warna
                              where w.Id == postId && w.RowStatus == true
                              select new WarnaViewModel
                              {
                                  Id = w.Id,
                                  Name = w.Name,
                                  Description = w.Description,
                                  Created = w.Created,
                                  CreatedBy = w.CreatedBy,
                                  RowStatus = w.RowStatus
                              }).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<long> AddPost(Warna warna)
        {
            if (db != null)
            {
                await db.Warna.AddAsync(warna);
                await db.SaveChangesAsync();

                return warna.Id;
            }

            return 0;
        }

        public async Task<long> DeletePost(long postId)
        {
            int result = 0;

            if (db != null)
            {
                //Find the warna for specific warna id
                var warna = await db.Warna.FirstOrDefaultAsync(x => x.Id == postId);

                if (warna != null)
                {
                    warna.RowStatus = false;
                    //Delete that warna
                    db.Warna.Update(warna);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<bool> UpdatePost(Warna warna)
        {
            bool result = false;
            if (db != null)
            {
                //Delete that warna
                db.Warna.Update(warna);

                //Commit the transaction
                await db.SaveChangesAsync();
                result = true;
            }
            return result;
        }
    }
}
