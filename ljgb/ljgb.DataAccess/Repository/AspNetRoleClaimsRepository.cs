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
    public class AspNetRoleClaimsRepository : IAspNetRoleClaims
    {
        ljgbContext db;
        public AspNetRoleClaimsRepository(ljgbContext _db)
        {
            db = _db;
        }
        public async Task<long> AddPost(AspNetRoleClaims model)
        {
            if (db != null)
            {
                await db.AspNetRoleClaims.AddAsync(model);
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
                var model = await db.AspNetRoleClaims.FirstOrDefaultAsync(x => x.Id == ID);

                if (model != null)
                {
                    //Delete that warna
                 
                    db.AspNetRoleClaims.Remove(model);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<List<AspNetRoleClaimsViewModel>> GetAll()
        {

            if (db != null)
            {
                return await (from model in db.AspNetRoleClaims
                              
                              select new AspNetRoleClaimsViewModel
                              {
                                  ID = model.Id,
                                  ClaimType = model.ClaimType,
                                  ClaimValue = model.ClaimValue,
                                  RoleID = model.RoleId
                              }).ToListAsync();
            }

            return null;
        }

        public async Task<AspNetRoleClaimsViewModel> GetPost(long ID)
        {
            if (db != null)
            {
                return await (from model in db.AspNetRoleClaims
                              where model.Id == ID
                              select new AspNetRoleClaimsViewModel
                              {
                                  ID = model.Id,
                                  ClaimType = model.ClaimType,
                                  ClaimValue = model.ClaimValue,
                                  RoleID = model.RoleId
                              }).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<bool> UpdatePost(AspNetRoleClaims model)
        {
            bool result = false;
            if (db != null)
            {
                try
                {
                    //Delete that warna
                    db.AspNetRoleClaims.Update(model);

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
