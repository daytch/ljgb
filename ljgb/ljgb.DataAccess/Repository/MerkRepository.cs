using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ljgb.DataAccess.Repository
{
    public class MerkRepository : IMerk
    {

        ljgbContext db;
        public MerkRepository(ljgbContext _db)
        {
            db = _db;
        }
        public async Task<long> AddPost(Merk model)
        {
            if (db != null)
            {
                await db.Merk.AddAsync(model);
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
                var model = await db.Merk.FirstOrDefaultAsync(x => x.Id == ID);

                if (model != null)
                {
                    model.RowStatus = false;
                    //Delete that warna
                    db.Merk.Update(model);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<List<MerkViewModel>> GetAll()
        {

            if (db != null)
            {
                return await (from model in db.Merk
                              where model.RowStatus == true
                              select new MerkViewModel
                              {
                                  ID = model.Id,
                                  Name = model.Nama,
                                  Description = model.Description,
                                  Created = model.Created,
                                  Createdby = model.CreatedBy,
                                  Modified = model.Modified,
                                  ModifiedBy = model.ModifiedBy,
                                  RowStatus = model.RowStatus
                              }).ToListAsync();
            }

            return null;
        }

        public async Task<MerkViewModel> GetPost(long ID)
        {
            if (db != null)
            {
                return await (from model in db.Merk
                              where model.Id == ID && model.RowStatus ==true
                              select new MerkViewModel
                              {
                                  ID = model.Id,
                                  Name = model.Nama,
                                  Description = model.Description,
                                  Created = model.Created,
                                  Createdby = model.CreatedBy,
                                  Modified = model.Modified,
                                  ModifiedBy = model.ModifiedBy,
                                  RowStatus = model.RowStatus
                              }).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<bool> UpdatePost(Merk model)
        {
            bool result = false;
            if (db != null)
            {
                try
                {
                    //Delete that warna
                    db.Merk.Update(model);

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
