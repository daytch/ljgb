using ljgb.DataAccess.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
   public interface IUserDetail
    {
        Task<long> SaveUserDetail(UserDetail detail);
        Task<UserDetail> SelectByUserProfileID(long id);
        Task<bool> Update(UserDetail detail);
    }
}
