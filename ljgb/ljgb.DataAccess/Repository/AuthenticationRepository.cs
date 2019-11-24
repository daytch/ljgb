using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Repository
{
    public class AuthenticationRepository : IAuthentication
    {
        ljgbContext db;

        public AuthenticationRepository(ljgbContext _db)
        {
            db = _db;
        }

        public async Task<UserProfile> GetUserProfileByEmail(string email)
        {
            UserProfile u = new UserProfile();
            if (db != null)
            {
                u = await db.UserProfile.FirstOrDefaultAsync(x => x.RowStatus == true && x.Email == email);
            }
            return u;
        }

        public async Task<UserProfile> GetUserProfile(UserProfile user)
        {
            UserProfile u = new UserProfile();
            if (db != null)
            {
                u = await db.UserProfile.FirstOrDefaultAsync(x => x.RowStatus == true && x.Email == user.Email && x.Password == user.Password);
            }
            return u;
        }

        public async Task<bool> IsUserActive(string email)
        {
            bool result = false;
            UserProfile u = new UserProfile();
            if (db != null)
            {
                u = await db.UserProfile.FirstOrDefaultAsync(x => x.RowStatus == true && x.Email == email);
                if (u != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public async Task<long> Save(UserProfile user)
        {
            long result = 0;

            if (db != null)
            {
                try
                {
                    await db.UserProfile.AddAsync(user);
                    result = await db.SaveChangesAsync();

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
