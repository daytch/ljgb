using ljgb.DataAccess.Models;
using ljgb.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
    }
}
