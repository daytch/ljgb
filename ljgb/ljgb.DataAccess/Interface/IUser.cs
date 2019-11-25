using ljgb.DataAccess.Model;
using ljgb.Common.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using ljgb.Common.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

namespace ljgb.DataAccess.Interface
{
    public interface IUser
    {
        Task<UserProfile> Select(long id);

        Task<List<UserProfile>> GetUserProfiles();

        Task<List<vw_salesman>> GetSalesman();

        Task<List<vw_buyer>> GetBuyer();

        Task<sp_GetUserDetail> GetSalesmanById(int id);

        Task<List<UserProfileViewModel>> GetPosts();

        Task<UserProfile> GetPost(string Email);
                
        Task<long> AddPost(UserProfile userProfile);

        Task<long> DeletePost(long UserProfileID);

        Task<bool> Update(UserProfile userProfile);

        Task<IdentityResult> Register(UserRequest userProfile);

        Task<string> GenerateEmailConfirmationToken(UserRequest userProfile);

        Task<bool> SendConfirmationEmail(UserRequest userProfile);

        Task<bool> SignIn(UserRequest userProfile);
        

        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemes();

        Task<SignInResult> PasswordSignIn(UserRequest userProfile);
        Task<UserProfile> GetUserByEmail(string Email);
    }
}
