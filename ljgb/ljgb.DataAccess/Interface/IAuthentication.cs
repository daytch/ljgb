using ljgb.DataAccess.Model;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface IAuthentication
    {
        Task<UserProfile> GetUserProfile(UserProfile user);

        Task<bool> IsUserActive(string email);

        Task<long> Save(UserProfile user);
    }
}
