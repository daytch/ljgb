using ljgb.Common.Requests;
using ljgb.Common.ViewModel;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ljgb.BusinessLogic
{
    public class UserFacade
    {
        #region Important
        private ljgbContext db;
        private IUser dep;
        private UserManager<IdentityUser> userManager;

        public UserFacade(UserManager<IdentityUser> _userManager)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            userManager = _userManager;

            db = new ljgbContext(optionsBuilder.Options);
            dep = new UserProfileRepository(db, userManager);
        }
        #endregion

        public async Task<List<UserProfile>> GetAllUser()
        {
            var categories = await dep.GetUserProfiles();
            if (categories == null)
            {
                return null;
            }
            return categories;
        }

        public async Task<List<UserProfileViewModel>> GetPosts()
        {
            var posts = await dep.GetPosts();
            if (posts == null)
            {
                return null;
            }

            return posts;
        }

        public async Task<UserProfileViewModel> GetPost(long postId)
        {
            var post = await dep.GetPost(postId);

            if (post == null)
            {
                return null;
            }
            return post;
        }

        public async Task<long> AddPost(UserProfile model)
        {
            var postId = await dep.AddPost(model);
            if (postId > 0)
            {
                return postId;
            }
            else
            {
                return 0;
            }
        }

        public async Task<long> DeletePost(long postId)
        {
            long result = 0;
            result = await dep.DeletePost(postId);
            if (result == 0)
            {
                return 0;
            }
            return result;
        }

        public async Task<bool> UpdatePost(UserProfile profile)
        {
            return await dep.UpdatePost(profile);
        }

        public async Task<IdentityResult> Register(UserRequest model)
        {
            IdentityResult result = new IdentityResult();
            try
            {
                result = await dep.Register(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
