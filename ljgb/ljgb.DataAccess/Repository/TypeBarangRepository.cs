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
    public class TypeBarangRepository :ITypeBarang
    {
        ljgbContext db;
        public TypeBarangRepository(ljgbContext _db)
        {
            db = _db;
        }
        public async Task<long> AddPost(TypeBarang model)
        {
            if (db != null)
            {
                await db.TypeBarang.AddAsync(model);
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
                var model = await db.TypeBarang.FirstOrDefaultAsync(x => x.Id == ID);

                if (model != null)
                {
                    model.RowStatus = false;
                    //Delete that warna
                    db.TypeBarang.Update(model);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<List<TypeBarangViewModel>> GetAll()
        {

            if (db != null)
            {
                return await (from model in db.TypeBarang
                              where model.RowStatus == true
                              select new TypeBarangViewModel
                              {
                                  ID = model.Id,
                                  Name = model.Name,
                                  ModelBarangID = model.ModelBarangId,
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

        public async Task<TypeBarangViewModel> GetPost(long ID)
        {
            if (db != null)
            {
                return await (from model in db.TypeBarang
                              where model.Id == ID && model.RowStatus == true
                              select new TypeBarangViewModel
                              {
                                  ID = model.Id,
                                  Name = model.Name,
                                  ModelBarangID = model.ModelBarangId,
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

        public async Task<bool> UpdatePost(TypeBarang model)
        {
            bool result = false;
            if (db != null)
            {
                try
                {
                    //Delete that warna
                    db.TypeBarang.Update(model);

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
