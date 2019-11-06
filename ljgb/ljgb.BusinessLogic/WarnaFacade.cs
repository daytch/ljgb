using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Repository;
using ljgb.DataAccess.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using ljgb.Common.ViewModel;
using ljgb.Common.Responses;
using System.Linq;
using System;

namespace ljgb.BusinessLogic
{
    public class WarnaFacade
    {
        #region Important
        private ljgbContext db;
        private IWarna dep;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        private static WarnaResponse SortByColumnWithOrder(string order, string orderDir, WarnaResponse resp)
        {
            try
            {
                // Sorting    
                switch (order)
                {
                    case "0":
                        // Setting.    
                        resp.data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? resp.data.OrderByDescending(p => p.Name).ToList()
                                                             : resp.data.OrderBy(p => p.Name).ToList();
                        break;
                    case "1":
                        // Setting.    
                        resp.data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? resp.data.OrderByDescending(p => p.Description).ToList()
                                                             : resp.data.OrderBy(p => p.Description).ToList();
                        break;

                    default:
                        // Setting.    
                        resp.data = resp.data.OrderByDescending(p => p.Id).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error("CompanyFacade.SortByColumnWithOrder :" + ex.ToString());
            }
            // info.    
            return resp;
        }

        public async Task<WarnaResponse> GetCategories(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            WarnaResponse resp = new WarnaResponse();
            List<Warna> Listwarna = await dep.GetWarna();
            resp.data = Listwarna.Select(x => new WarnaViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                Modified = x.Modified,
                ModifiedBy = x.ModifiedBy,
                RowStatus = x.RowStatus
            }).ToList();
            // Total record count.    
            int totalRecords = resp.data.Count;
            // Verification.    
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search    
                resp.data = resp.data.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                            p.Description.ToLower().Contains(search.ToLower())).ToList();
            }
            // Sorting.    
            resp = SortByColumnWithOrder(order, orderDir, resp);
            // Filter record count.    
            int recFilter = resp.data.Count;

            resp.data = resp.data.Skip(startRec).Take(pageSize).ToList();

            resp.draw = Convert.ToInt32(draw);
            resp.recordsTotal = totalRecords;
            resp.recordsFiltered = recFilter;

            return resp;
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

        public async Task<WarnaResponse> GetAllWithoutFilter()
        {
            WarnaResponse response = new WarnaResponse();
            response.data =  await dep.GetAllWithoutFilter();
            response.IsSuccess = true;
            return response;
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
