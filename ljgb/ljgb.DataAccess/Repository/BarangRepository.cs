using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using ljgb.DataAccess.ViewModel;

namespace ljgb.DataAccess.Repository
{
    public class BarangRepository : IBarang
    {
        ljgbContext db;
        public BarangRepository(ljgbContext _db)
        {
            db = _db;
        }
        public async Task<long> AddPost(Barang model)
        {
            if (db != null)
            {
                await db.Barang.AddAsync(model);
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
                var model = await db.Barang.FirstOrDefaultAsync(x => x.Id == ID);

                if (model != null)
                {
                    //Delete that warna
                    model.RowStatus = false;
                    db.Barang.Update(model);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<List<BarangViewModel>> GetAll()
        {

            if (db != null)
            {
                return await (from model in db.Barang
                              where model.RowStatus == true
                              select new BarangViewModel
                              {
                                  ID = model.Id,
                                  Name = model.Name,
                                  HargaOTR = model.HargaOtr,
                                  PhotoPath = model.PhotoPath,
                                  TypeBarangID = model.TypeBarangId,
                                  WarnaID = model.WarnaId,
                                  Created = model.Created,
                                  CreadtedBy = model.CreatedBy,
                                  Modified = model.Modified,
                                  ModifiedBy = model.ModifiedBy,
                                  RowStatus = model.RowStatus
                              }).ToListAsync();
            }

            return null;
        }

        public async Task<BarangViewModel> GetPost(long ID)
        {
            if (db != null)
            {
                return await (from model in db.Barang
                              where model.Id == ID &&  model.RowStatus == true
                              select new BarangViewModel
                              {
                                  ID = model.Id,
                                  Name = model.Name,
                                  HargaOTR = model.HargaOtr,
                                  PhotoPath = model.PhotoPath,
                                  TypeBarangID = model.TypeBarangId,
                                  WarnaID = model.WarnaId,
                                  Created = model.Created,
                                  CreadtedBy = model.CreatedBy,
                                  Modified = model.Modified,
                                  ModifiedBy = model.ModifiedBy,
                                  RowStatus = model.RowStatus
                              }).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<bool> UpdatePost(Barang model)
        {
            bool result = false;
            if (db != null)
            {
                try
                {
                    //Delete that warna
                    db.Barang.Update(model);

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
