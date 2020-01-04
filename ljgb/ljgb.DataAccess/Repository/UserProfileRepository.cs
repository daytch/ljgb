using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.Common.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ljgb.Common.Requests;
using Microsoft.AspNetCore.Authentication;

namespace ljgb.DataAccess.Repository
{
    public class UserProfileRepository : IUser
    {
        ljgbContext db;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly SignInManager<IdentityUser> signInManager;
        public UserProfileRepository(ljgbContext _db, UserManager<IdentityUser> _userManager, IEmailSender _emailSender, SignInManager<IdentityUser> _signInManager)
        {
            db = _db;
            userManager = _userManager;
            emailSender = _emailSender;
            signInManager = _signInManager;
        }
        
        public async Task<sp_GetUserDetail> GetSalesmanById(int id)
        {
            if (db != null)
            {
                return await db.Set<sp_GetUserDetail>().FromSql("EXEC sp_GetUserDetail {0}", id).AsNoTracking().FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<List<UserProfile>> GetUserProfiles()
        {
            if (db != null)
            {
                return await db.UserProfile.ToListAsync();
            }

            return null;
        }

        public async Task<List<vw_salesman>> GetSalesman()
        {
            if (db != null)
            {
                return await db.vw_salesman.ToListAsync();
            }

            return null;
        }

        public async Task<List<vw_buyer>> GetBuyer()
        {
            if (db != null)
            {
                return await db.vw_buyer.ToListAsync();
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

        public async Task<UserProfile> GetPost(string Email)
        {
            if (db != null)
            {
                return await db.UserProfile.Where(x => x.RowStatus == true && x.Email == Email).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<UserProfile> Select(long id)
        {
            if (db != null)
            {
                return await db.UserProfile.Where(x => x.Id == id && x.RowStatus == true).FirstOrDefaultAsync();
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

        public async Task<bool> Update(UserProfile user)
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
                result = await userManager.CreateAsync(user.user, user.Password);
                //await userManager.UpdateSecurityStampAsync(user.user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<string> GenerateEmailConfirmationToken(UserRequest user)
        {
            string result = string.Empty;
            try
            {
                result = await userManager.GenerateEmailConfirmationTokenAsync(user.user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<bool> SendConfirmationEmail(UserRequest user)
        {
            bool result = true;
            try
            {
                // _emailSender.SendEmailAsync
                await emailSender.SendEmailAsync(user.user.Email, user.EmailSubject, user.HTMLTag);
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            return result;
        }

        public async Task<bool> SignIn(UserRequest user)
        {
            bool result = true;
            try
            {
                user.user.SecurityStamp = Guid.NewGuid().ToString();
                await signInManager.SignInAsync(user.user, false);
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            return result;
        }
        
        public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemes()
        {
            IEnumerable<AuthenticationScheme> result;
            try
            {
                result = await signInManager.GetExternalAuthenticationSchemesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<SignInResult> PasswordSignIn(UserRequest user)
        {
            SignInResult result = new SignInResult();
            try
            {
                result = await signInManager.PasswordSignInAsync(user.user.Email, user.Password, user.RememberMe, user.lockoutOnFailure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<UserProfile> GetUserByEmail(string Email)
        {
            try
            {
                return await db.UserProfile.Where(x => x.RowStatus == true && x.Email == Email).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //public Task<bool> UpdatePassword(UserProfile userProfile)
        //{
        //    bool result = true;
        //    try
        //    {
        //        var usrProfile = db.UserProfile.Where(x=>x.RowStatus == true && x.Email == userProfile.email)
        //    }
        //    catch (Exception ex)
        //    {
        //        result = false;
        //        throw ex;
        //    }
        //    return result;
        //}
    }
}
