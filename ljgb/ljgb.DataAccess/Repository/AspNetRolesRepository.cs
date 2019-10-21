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
    class AspNetRolesRepository
    {
        public class BarangRepository : IAspNetRoles
        {
            ljgbContext db;
            public BarangRepository(ljgbContext _db)
            {
                db = _db;
            }
            public async Task<string> AddPost(AspNetRoles model)
            {
                if (db != null)
                {
                    await db.AspNetRoles.AddAsync(model);
                    await db.SaveChangesAsync();

                    return model.Id;
                }

                return "0";
            }

            public async Task<string> DeletePost(string ID)
            {
                string result = "0";

                if (db != null)
                {
                    //Find the warna for specific userprofile
                    var model = await db.AspNetRoles.FirstOrDefaultAsync(x => x.Id == ID);

                    if (model != null)
                    {
                        //Delete that warna

                        db.AspNetRoles.Remove(model);

                        //Commit the transaction
                        result = (await db.SaveChangesAsync()).ToString();
                    }
                    return result;
                }

                return result;
            }

            public async Task<List<AspNetRolesViewModel>> GetAll()
            {

                if (db != null)
                {
                    return await (from model in db.AspNetRoles

                                  select new AspNetRolesViewModel
                                  {
                                      ID = model.Id,
                                      Name = model.Name,
                                      CocurrencyStamp = model.ConcurrencyStamp,
                                      NormalizeName = model.NormalizedName
                                  }).ToListAsync();
                }

                return null;
            }

            public async Task<AspNetRolesViewModel> GetPost(long ID)
            {
                if (db != null)
                {
                    return await (from model in db.AspNetRoles

                                  select new AspNetRolesViewModel
                                  {
                                      ID = model.Id,
                                      Name = model.Name,
                                      CocurrencyStamp = model.ConcurrencyStamp,
                                      NormalizeName = model.NormalizedName
                                  }).FirstOrDefaultAsync();
                }

                return null;
            }

            public async Task<bool> UpdatePost(AspNetRoles model)
            {
                bool result = false;
                if (db != null)
                {
                    try
                    {
                        //Delete that warna
                        db.AspNetRoles.Update(model);

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
}
