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
    public class RoleRepository : IRole
    {
        ljgbContext db;
        public RoleRepository(ljgbContext _db)
        {
            db = _db;
        }
        public async Task<long> AddPost(Role role)
        {
            if (db != null)
            {
                await db.Role.AddAsync(role);
                await db.SaveChangesAsync();

                return role.Id;
            }

            return 0;
        }

        public async Task<long> DeletePost(long roleID)
        {
            int result = 0;

            if (db != null)
            {
                //Find the warna for specific userprofile
                var role = await db.Role.FirstOrDefaultAsync(x => x.Id == roleID);

                if (role != null)
                {
                    role.RowStatus = false;
                    //Delete that warna
                    db.Role.Update(role);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<List<RoleViewModel>> GetAllRole()
        {

            if (db != null)
            {
                return await (from role in db.Role
                              where role.RowStatus == true
                              select new RoleViewModel
                              {
                                  ID = role.Id,
                                  Name = role.Name,
                                  Description = role.Description,
                                  Created = role.Created,
                                  CreatedBy = role.CreatedBy,
                                  Modified = role.Modified,
                                  ModifiedBy = role.ModifiedBy,
                                  RowStatus = role.RowStatus
                              }).ToListAsync();
            }

            return null;
        }

        public async Task<RoleViewModel> GetPost(long postId)
        {
            if (db != null)
            {
                return await (from role in db.Role
                              where role.Id == postId && role.RowStatus == true
                              select new RoleViewModel
                              {
                                  ID = role.Id,
                                  Name = role.Name,
                                  Description = role.Description,
                                  Created = role.Created,
                                  CreatedBy = role.CreatedBy,
                                  Modified = role.Modified,
                                  ModifiedBy = role.ModifiedBy,
                                  RowStatus = role.RowStatus
                              }).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<bool> UpdatePost(Role role)
        {
            bool result = false;
            if (db != null)
            {
                try
                {
                    //Delete that warna
                    db.Role.Update(role);

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
