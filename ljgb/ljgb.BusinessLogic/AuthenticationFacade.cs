using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ljgb.BusinessLogic
{
    public class AuthenticationFacade
    {
        #region Important
        private ljgbContext db;
        private IAuthentication dataAccess;
        private IUser userDA;
        private Security security;

        //private readonly UserManager<IdentityUser> userManager;
        //private readonly IEmailSender emailSender;
        //private readonly SignInManager<IdentityUser> signInManager;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AuthenticationFacade()//UserManager<IdentityUser> _userManager, IEmailSender _emailSender, SignInManager<IdentityUser> _signInManager)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            security = new Security();

            db = new ljgbContext(optionsBuilder.Options);
            dataAccess = new AuthenticationRepository(db);
            userDA = new UserProfileRepository(db);
        }
        #endregion
        public async Task<AuthenticationResponse> Login(UserRequest user)
        {
            AuthenticationResponse response = new AuthenticationResponse();
            UserProfile userProfile = new UserProfile();
            try
            {
                byte[] pass = security.GetSHA1(user.Email, user.Password);
                UserProfile up = new UserProfile()
                {
                    Email = user.Email,
                    Password = pass
                };

                userProfile = await dataAccess.GetUserProfile(up);
                if (userProfile != null)
                {
                    if (userProfile.IsActivated == true)
                    {
                        response.Name = userProfile.Nama;
                        response.Token = security.GenerateToken(user.Email);
                        response.Message = "Success";
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.Name = userProfile.Nama;
                        response.Token = security.GenerateToken(user.Email);
                        response.Message = "Your Account is not active";
                        response.IsSuccess = false;
                    }
                   
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Please check your email and password.";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return response;
        }

        public string GenerateToken(string email)
        {
            string Token = string.Empty;
            try
            {
                Token = security.GenerateToken(email);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return Token;
        }

        public bool IsUserActive(string email)
        {
            bool result = false;

            result = dataAccess.IsUserActive(email).Result;

            return result;
        }

        public async Task<AuthenticationResponse> Register(UserRequest user)
        {
            AuthenticationResponse response = new AuthenticationResponse();
            UserProfile userProfile = new UserProfile();
            try
            {
                byte[] pass = security.GetSHA1(user.Email, user.Password);
                UserProfile up = new UserProfile()
                {
                    Email = user.Email,
                    Password = pass,
                    Nama = user.FirstName + " " + user.LastName,
                    Telp = user.Telp,

                    Created = DateTime.Now,
                    CreatedBy = user.Email,
                    RowStatus = true,
                    IsActivated = user.IsAdminPortal
                };
                UserProfile usr = await dataAccess.GetUserProfileByEmail(user.Email);
                if (usr != null)
                {
                    if (usr.IsActivated == true)
                    {
                        response.IsSuccess = false;
                        response.Message = "Failed! Email has been registered.";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Failed! Email has been registered and Check your email to active your account.";
                    }
                }
                else
                {
                    long id = await dataAccess.Save(up);
                    if (id > 0)
                    {
                        //response.Token = security.GenerateToken(user.Email);
                        response.Message = "Success created user profile.";
                        response.IsSuccess = true;

                        #region Sent
                        EmailSender emailSender = new EmailSender();

                        #endregion
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Failed when save userprofile.";
                    }
                }
              
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return response;
        }

        public async Task<AuthenticationResponse> ForgotPassword(UserRequest user)
        {
            AuthenticationResponse response = new AuthenticationResponse();
            UserProfile userProfile = new UserProfile();
            try
            {
                userProfile = await userDA.GetPost(user.Email);
                if (userProfile != null)
                {
                    response.Name = userProfile.Nama;
                    response.Message = "This account is active.";
                    response.IsSuccess = true;                    
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Email does not exist in our database.";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return response;
        }
        
        public async Task<bool> UpdatePassword(UserRequest request)
        {
            bool result = true;
            try
            {
                Security sec = new Security();
                UserProfile user = await userDA.GetUserByEmail(request.Email);

                user.Email = request.Email;
                user.Password = sec.GetSHA1(request.Email, request.RePassword);

                result = await userDA.Update(user);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }

            return result;
        }

        public async Task<bool> ActivateAccount(UserRequest request)
        {
            bool result = true;
            try
            {
                Security sec = new Security();
                UserProfile user = await userDA.GetUserByEmail(request.Email);

                user.Email = request.Email;
                user.IsActivated = true;

                result = await userDA.Update(user);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }

            return result;
        }
    }
}
