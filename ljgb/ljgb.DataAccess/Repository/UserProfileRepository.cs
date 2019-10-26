using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.Common.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using ljgb.Common.Requests;

namespace ljgb.DataAccess.Repository
{
    public class UserProfileRepository : IUser
    {
        ljgbContext db;
        private readonly UserManager<IdentityUser> userManager;
        public UserProfileRepository(ljgbContext _db, UserManager<IdentityUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        public async Task<List<UserProfile>> GetUserProfiles()
        {
            if (db != null)
            {
                return await db.UserProfile.ToListAsync();
            }

            return null;
        }

        public async Task<List<UserProfileViewModel>> GetPosts()
        {
            if (db != null)
            {
                return await (from user in db.UserProfile
                              where user.RowStatus == true
                              select new UserProfileViewModel
                              {
                                  ID = user.Id,
                                  Name = user.Nama,
                                  Facebook = user.Facebook,
                                  IG = user.Ig,
                                  JenisKelamin = user.JenisKelamin,
                                  Telp = user.Telp,
                                  Email = user.Email,
                                  Created = user.Created,
                                  CreatedBy = user.CreatedBy,
                                  Modified = user.Modified,
                                  ModifiedBy = user.ModifiedBy,
                                  RowStatus = user.RowStatus
                              }).ToListAsync();
            }

            return null;
        }

        public async Task<UserProfileViewModel> GetPost(long postId)
        {
            if (db != null)
            {
                return await (from user in db.UserProfile
                              where user.Id == postId && user.RowStatus == true
                              select new UserProfileViewModel
                              {
                                  ID = user.Id,
                                  Name = user.Nama,
                                  Facebook = user.Facebook,
                                  IG = user.Ig,
                                  JenisKelamin = user.JenisKelamin,
                                  Telp = user.Telp,
                                  Email = user.Email,
                                  Created = user.Created,
                                  CreatedBy = user.CreatedBy,
                                  Modified = user.Modified,
                                  ModifiedBy = user.ModifiedBy,
                                  RowStatus = user.RowStatus
                              }).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<long> AddPost(UserProfile user)
        {
            if (db != null)
            {
                await db.UserProfile.AddAsync(user);
                await db.SaveChangesAsync();

                return user.Id;
            }

            return 0;
        }

        public async Task<long> DeletePost(long postId)
        {
            int result = 0;

            if (db != null)
            {
                //Find the warna for specific userprofile
                var user = await db.UserProfile.FirstOrDefaultAsync(x => x.Id == postId);

                if (user != null)
                {
                    //Delete that warna
                    db.UserProfile.Remove(user);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<bool> UpdatePost(UserProfile user)
        {
            bool result = false;
            if (db != null)
            {
                try
                {
                    //Delete that warna
                    db.UserProfile.Update(user);

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

        public Task<List<UserProfile>> GetUser()
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> Register(UserRequest user)
        {
            IdentityResult result = new IdentityResult();
            try
            {
                result = await userManager.CreateAsync(user.user, user.password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
