using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Repository;
using ljgb.DataAccess.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using ljgb.Common.ViewModel;

namespace ljgb.BusinessLogic
{
    public class WarnaFacade
    {
        #region Important
        private ljgbContext db;
        private IWarna dep;

        public WarnaFacade()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            db = new ljgbContext(optionsBuilder.Options);
            this.dep = new WarnaRepository(db);
        }
        #endregion
        
        public async Task<List<Warna>> GetCategories()
        {
            var categories = await dep.GetWarna();
            if (categories == null)
            {
                return null;
            }
            return categories;
        }

        public async Task<List<WarnaViewModel>> GetPosts()
        {
            var posts = await dep.GetPosts();
            if (posts == null)
            {
                return null;
            }

            return posts;
        }

        public async Task<WarnaViewModel> GetPost(long postId)
        {
            var post = await dep.GetPost(postId);

            if (post == null)
            {
                return null;
            }
            return post;
        }

        public async Task<long> AddPost(Warna model)
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

        public async Task<bool> UpdatePost(Warna model)
        {
            bool result = await dep.UpdatePost(model);

            return result;
        }

    }
}
