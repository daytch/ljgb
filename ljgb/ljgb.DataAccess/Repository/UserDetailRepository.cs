using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.Common.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace ljgb.DataAccess.Repository
{
    public class UserDetailRepository : IUserDetail
    {
        ljgbContext db;
        public UserDetailRepository(ljgbContext _db)
        {
            db = _db;
        }

        public async Task<long> SaveUserDetail(UserDetail detail)
        {
            if (db != null)
            {
                try
                {
                    await db.UserDetail.AddAsync(detail);
                    await db.SaveChangesAsync();

                    return detail.Id;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return 0;
        }

        public async Task<UserDetail> SelectByUserProfileID(long id)
        {
            UserDetail result = new UserDetail();
            if (db != null)
            {
                try
                {
                    result = await db.UserDetail.Where(x => x.RowStatus == true && x.UserProfileId == id).FirstOrDefaultAsync();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            return result;
        }

        public async Task<bool> Update(UserDetail user)
        {
            bool result = false;
            if (db != null)
            {
                try
                {
                    //Delete that warna
                    db.UserDetail.Update(user);

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
