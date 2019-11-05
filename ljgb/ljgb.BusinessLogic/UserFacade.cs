using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.Common.ViewModel;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ljgb.BusinessLogic
{
    public class UserFacade
    {
        #region Important
        private ljgbContext db;
        private IUser dep;
        private IUserDetail IDetail;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly SignInManager<IdentityUser> signInManager;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public UserFacade(UserManager<IdentityUser> _userManager, IEmailSender _emailSender, SignInManager<IdentityUser> _signInManager)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            userManager = _userManager;
            emailSender = _emailSender;
            signInManager = _signInManager;

            db = new ljgbContext(optionsBuilder.Options);
            dep = new UserProfileRepository(db, userManager, emailSender, signInManager);
            IDetail = new UserDetailRepository(db);
        }
        #endregion

        private static UserResponse SortByColumnWithOrder(string order, string orderDir, UserResponse resp)
        {
            try
            {
                if (resp.databuyer == null)
                {
                    #region Sorting Salesman   
                    switch (order)
                    {
                        case "0":
                            // Setting.    
                            resp.datasalesman = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? resp.datasalesman.OrderByDescending(p => p.Nama).ToList()
                                                                 : resp.datasalesman.OrderBy(p => p.Nama).ToList();
                            break;
                        case "1":
                            // Setting.    
                            resp.datasalesman = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? resp.datasalesman.OrderByDescending(p => p.Email).ToList()
                                                                 : resp.datasalesman.OrderBy(p => p.Email).ToList();
                            break;

                        case "2":
                            // Setting.    
                            resp.datasalesman = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? resp.datasalesman.OrderByDescending(p => p.Telp).ToList()
                                                                 : resp.datasalesman.OrderBy(p => p.Telp).ToList();
                            break;
                        case "3":
                            // Setting.    
                            resp.datasalesman = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? resp.datasalesman.OrderByDescending(p => p.VerifiedDate).ToList()
                                                                 : resp.datasalesman.OrderBy(p => p.VerifiedDate).ToList();
                            break;
                        case "4":
                            // Setting.    
                            resp.datasalesman = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? resp.datasalesman.OrderByDescending(p => p.VerifiedBy).ToList()
                                                                 : resp.datasalesman.OrderBy(p => p.VerifiedBy).ToList();
                            break;
                        default:
                            // Setting.    
                            resp.datasalesman = resp.datasalesman.OrderByDescending(p => p.ID).ToList();
                            break;
                    }
                    #endregion
                }
                else
                {
                    #region Sorting Buyer
                    switch (order)
                    {
                        case "0":
                            // Setting.    
                            resp.databuyer = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? resp.databuyer.OrderByDescending(p => p.Nama).ToList()
                                                                 : resp.databuyer.OrderBy(p => p.Nama).ToList();
                            break;
                        case "1":
                            // Setting.    
                            resp.databuyer = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? resp.databuyer.OrderByDescending(p => p.Email).ToList()
                                                                 : resp.databuyer.OrderBy(p => p.Email).ToList();
                            break;

                        case "2":
                            // Setting.    
                            resp.databuyer = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? resp.databuyer.OrderByDescending(p => p.Telp).ToList()
                                                                 : resp.databuyer.OrderBy(p => p.Telp).ToList();
                            break;
                        case "3":
                            // Setting.    
                            resp.databuyer = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? resp.databuyer.OrderByDescending(p => p.VerifiedDate).ToList()
                                                                 : resp.databuyer.OrderBy(p => p.VerifiedDate).ToList();
                            break;
                        case "4":
                            // Setting.    
                            resp.databuyer = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? resp.databuyer.OrderByDescending(p => p.VerifiedBy).ToList()
                                                                 : resp.databuyer.OrderBy(p => p.VerifiedBy).ToList();
                            break;
                        default:
                            // Setting.    
                            resp.databuyer = resp.databuyer.OrderByDescending(p => p.ID).ToList();
                            break;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                log.Error("UserFacade.SortByColumnWithOrder :" + ex.ToString());
            }
            // info.    
            return resp;
        }
        public async Task<List<UserProfile>> GetAllUser()
        {
            var categories = await dep.GetUserProfiles();
            if (categories == null)
            {
                return null;
            }
            return categories;
        }

        public async Task<UserResponse> GetAllSalesman(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            UserResponse resp = new UserResponse();
            List<vw_salesman> ListSalesman = await dep.GetSalesman();
            resp.datasalesman = ListSalesman.Select(x => new SalesmanViewModel()
            {
                ID = x.ID,
                DetailID = x.DetailID,
                Nama = x.Nama,
                Email = x.Email,
                Telp = x.Telp,
                VerifiedBy = x.VerifiedBy,
                VerifiedDate = x.VerifiedDate
            }).ToList();

            int totalRecords = resp.datasalesman.Count;
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                resp.datasalesman = resp.datasalesman.Where(p => p.Nama.ToString().ToLower().Contains(search.ToLower()) ||
                            p.Email.ToLower().Contains(search.ToLower()) ||
                            p.VerifiedBy.ToLower().Contains(search.ToLower())).ToList();
            }

            resp = SortByColumnWithOrder(order, orderDir, resp);

            int recFilter = resp.datasalesman.Count;

            resp.datasalesman = resp.datasalesman.Skip(startRec).Take(pageSize).ToList();

            resp.draw = Convert.ToInt32(draw);
            resp.recordsTotal = totalRecords;
            resp.recordsFiltered = recFilter;
            return resp;
        }

        public async Task<UserResponse> GetAllBuyer(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            UserResponse resp = new UserResponse();
            List<vw_buyer> ListBuyer = await dep.GetBuyer();
            resp.databuyer = ListBuyer.Select(x => new BuyerViewModel()
            {
                ID = x.ID,
                DetailID = x.DetailID,
                Nama = x.Nama,
                Email = x.Email,
                Telp = x.Telp,
                VerifiedBy = x.VerifiedBy,
                VerifiedDate = x.VerifiedDate
            }).ToList();

            int totalRecords = resp.databuyer.Count;
            // Verification.    
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search    
                resp.databuyer = resp.databuyer.Where(p => p.Nama.ToString().ToLower().Contains(search.ToLower()) ||
                            p.Email.ToLower().Contains(search.ToLower()) ||
                            p.VerifiedBy.ToLower().Contains(search.ToLower())).ToList();
            }
            // Sorting.    
            resp = SortByColumnWithOrder(order, orderDir, resp);

            int recFilter = resp.databuyer.Count;

            resp.databuyer = resp.databuyer.Skip(startRec).Take(pageSize).ToList();

            resp.draw = Convert.ToInt32(draw);
            resp.recordsTotal = totalRecords;
            resp.recordsFiltered = recFilter;
            return resp;
        }

        public async Task<UserDetailResponse> GetSalesmanById(int id)
        {
            var sales = await dep.GetSalesmanById(id);
            if (sales == null)
            {
                return null;
            }

            UserDetailResponse resp = new UserDetailResponse()
            {
                Id = sales.Id,
                Nama = sales.Nama,
                Alamat = sales.Alamat,
                Email = sales.Email,
                JenisKelamin = sales.JenisKelamin,
                KodeDealer = sales.KodeDealer,
                KotaId = sales.KotaId,
                ProvinsiId = sales.ProvinsiId,
                Telp = sales.Telp,
                IsSuccess = true,
                Message = "Success"
            };
            return resp;
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

        //public async Task<bool> DeleteUserDetail(UserRequest request)
        //{
        //    UserDetail detail = await dep.SelectUserDetail(request.DetailID);
        //    detail.Modified = DateTime.Now;
        //    detail.ModifiedBy = "Admin";
        //    detail.RowStatus = false;

        //    bool postId = await dep.SaveUserDetail(detail);
        //    return postId;
        //}

        public async Task<UserResponse> SaveSalesman(UserRequest model)
        {
            UserResponse response = new UserResponse();
            try
            {
                UserProfile user = new UserProfile();
                UserDetail detail = new UserDetail();

                #region Insert Profile
                if (model.Id < 1)
                {
                    user = new UserProfile()
                    {
                        Nama = model.Nama,
                        Email = model.Email,
                        Telp = model.Telp,
                        Alamat = model.Alamat,
                        KotaId = model.KotaId,
                        JenisKelamin = model.JenisKelamin,

                        Created = DateTime.Now,
                        CreatedBy = "Admin",
                        RowStatus = true
                    };

                    long userID = await dep.AddPost(user);
                    if (userID < 1)
                    {
                        response.IsSuccess = false;
                        response.Message = "Failed When save to table UserProfile";
                        return response;
                    }
                }
                else
                {
                    user = await dep.Select(Convert.ToInt64(model.Id));
                    if (user == null)
                    {
                        response.IsSuccess = false;
                        response.Message = "Failed When get UserProfile.";
                        return response;
                    }

                    user.Nama = model.Nama;
                    user.Telp = model.Telp;
                    user.Alamat = model.Alamat;
                    user.KotaId = model.KotaId;

                    user.Modified = DateTime.Now;
                    user.ModifiedBy = "Admin";
                    bool result = await dep.Update(user);
                    if (!result)
                    {
                        response.IsSuccess = false;
                        response.Message = "Failed When save to table UserProfile";
                        return response;
                    }
                }
                #endregion

                #region Insert UserDetail
                detail = await IDetail.SelectByUserProfileID(user.Id);
                if (detail == null)
                {
                    detail = new UserDetail()
                    {
                        UserProfileId = Convert.ToInt32(user.Id),
                        VerifiedBy = "Admin",
                        VerifiedDate = DateTime.Now,
                        Description = "seller",
                        KodeDealer = model.KodeDealer,

                        Created = DateTime.Now,
                        CreatedBy = "Admin",
                        RowStatus = true
                    };
                    long DetailID = await IDetail.SaveUserDetail(detail);
                    if (DetailID < 1)
                    {
                        response.IsSuccess = false;
                        response.Message = "Failed When save to table UserDetail";
                        return response;
                    }
                }
                else
                {
                    detail.UserProfileId = Convert.ToInt32(user.Id);
                    detail.VerifiedBy = "Admin";
                    detail.VerifiedDate = DateTime.Now;
                    detail.Description = "seller";
                    detail.KodeDealer = model.KodeDealer;

                    detail.Modified = DateTime.Now;
                    detail.ModifiedBy = "Admin";
                    bool result = await IDetail.Update(detail);
                    if (!result)
                    {
                        response.IsSuccess = false;
                        response.Message = "Failed When save to table UserDetail";
                        return response;
                    }
                }
                #endregion

                response.Message = "Save Userprofile & UserDetail Success.";
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
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

        public async Task<string> GenerateEmailConfirmationToken(UserRequest model)
        {
            string result = string.Empty;
            try
            {
                result = await dep.GenerateEmailConfirmationToken(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<bool> SendConfirmationEmail(UserRequest model)
        {
            bool result = true;
            try
            {
                result = await dep.SendConfirmationEmail(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<bool> SignIn(UserRequest model)
        {
            bool result = true;
            try
            {
                result = await dep.SignIn(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemes()
        {
            IEnumerable<AuthenticationScheme> result;
            try
            {
                result = await dep.GetExternalAuthenticationSchemes();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<SignInResult> PasswordSignIn(UserRequest model)
        {
            SignInResult result = new SignInResult();
            try
            {
                result = await dep.PasswordSignIn(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
