using ljgb.DataAccess.Model;
using ljgb.Common.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using ljgb.Common.Requests;
using Microsoft.AspNetCore.Identity;

namespace ljgb.DataAccess.Interface
{
    public interface IUser
    {
        Task<List<UserProfile>> GetUserProfiles();

        Task<List<UserProfileViewModel>> GetPosts();

        Task<UserProfileViewModel> GetPost(long postId);

        Task<long> AddPost(UserProfile userProfile);

        Task<long> DeletePost(long UserProfileID);

        Task<bool> UpdatePost(UserProfile userProfile);
        Task<IdentityResult> Register(UserRequest userProfile);
    }
}
