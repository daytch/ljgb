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
    public class HargaSalesmanRepository : IHargaSalesman
    {

        ljgbContext db;
        public HargaSalesmanRepository(ljgbContext _db)
        {
            db = _db;
        }
        public async Task<long> AddPost(HargaSalesman model)
        {
            if (db != null)
            {
                await db.HargaSalesman.AddAsync(model);
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
                var model = await db.HargaSalesman.FirstOrDefaultAsync(x => x.Id == ID);

                if (model != null)
                {
                    model.RowStatus = false;
                    //Delete that warna
                    db.HargaSalesman.Update(model);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<List<HargaSalesmanViewModel>> GetAll()
        {

            if (db != null)
            {
                return await (from model in db.HargaSalesman
                              where model.RowStatus == true
                              select new HargaSalesmanViewModel
                              {
                                  ID = model.Id,
                                  BarangID = model.BarangId,
                                  UserProfileID = model.UserProfileId,
                                  Discount = model.Discount,
                                  HargaFinal = model.HargaFinal,
                                  Created = model.Created,
                                  CreatedBy = model.Createdby,
                                  Modified = model.Modified,
                                  ModifiedBy = model.ModifiedBy,
                                  RowStatus = model.RowStatus
                              }).ToListAsync();
            }

            return null;
        }

        public async Task<HargaSalesmanViewModel> GetPost(long ID)
        {
            if (db != null)
            {
                return await (from model in db.HargaSalesman
                              where model.Id == ID && model.RowStatus == true
                              select new HargaSalesmanViewModel
                              {
                                  ID = model.Id,
                                  BarangID = model.BarangId,
                                  UserProfileID = model.UserProfileId,
                                  Discount = model.Discount,
                                  HargaFinal = model.HargaFinal,
                                  Created = model.Created,
                                  CreatedBy = model.Createdby,
                                  Modified = model.Modified,
                                  ModifiedBy = model.ModifiedBy,
                                  RowStatus = model.RowStatus
                              }).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<bool> UpdatePost(HargaSalesman model)
        {
            bool result = false;
            if (db != null)
            {
                try
                {
                    //Delete that warna
                    db.HargaSalesman.Update(model);

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
