using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Models;
using ljgb.DataAccess.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ljgb.DataAccess.Repository
{
    public class ModelBarangRepository : IModelBarang
    {
        ljgbContext db;
        public ModelBarangRepository(ljgbContext _db)
        {
            db = _db;
        }
        public async Task<long> AddPost(ModelBarang model)
        {
            if (db != null)
            {
                await db.ModelBarang.AddAsync(model);
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
                var model = await db.ModelBarang.FirstOrDefaultAsync(x => x.Id == ID);

                if (model != null)
                {
                    model.RowStatus = false;
                    //Delete that warna
                    db.ModelBarang.Update(model);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<List<ModelBarangViewModel>> GetAll()
        {

            if (db != null)
            {
                return await (from model in db.ModelBarang
                              where model.RowStatus == true
                              select new ModelBarangViewModel
                              {
                                  ID = model.Id,
                                  Name = model.Name,
                                  MerkID = model.MerkId,
                                  Description = model.Description,
                                  Created = model.Created,
                                  CreatedBy = model.Createdby,
                                  Modified = model.Modified,
                                  ModifiedBy = model.ModifiedBy,
                                  RowStatus = model.RowStatus
                              }).ToListAsync();
            }

            return null;
        }

        public async Task<ModelBarangViewModel> GetPost(long ID)
        {
            if (db != null)
            {
                return await (from model in db.ModelBarang
                              where model.Id == ID & model.RowStatus == true
                              select new ModelBarangViewModel
                              {
                                  ID = model.Id,
                                  Name = model.Name,
                                  MerkID = model.MerkId,
                                  Description = model.Description,
                                  Created = model.Created,
                                  CreatedBy = model.Createdby,
                                  Modified = model.Modified,
                                  ModifiedBy = model.ModifiedBy,
                                  RowStatus = model.RowStatus
                              }).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<bool> UpdatePost(ModelBarang model)
        {
            bool result = false;
            if (db != null)
            {
                try
                {
                    //Delete that warna
                    db.ModelBarang.Update(model);

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
