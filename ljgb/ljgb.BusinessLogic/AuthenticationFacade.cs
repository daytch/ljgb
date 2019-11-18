using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.Repository;
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
        private Security security;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AuthenticationFacade()
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
        }
        #endregion
        public async Task<AuthenticationResponse> Login(UserRequest user)
        {
            AuthenticationResponse response = new AuthenticationResponse();
            UserProfile userProfile = new UserProfile();

            byte[] pass = security.GetSHA1(user.Email, user.Password);
            UserProfile up = new UserProfile()
            {
                Email = user.Email,
                Password = pass
            };

            userProfile = await dataAccess.GetUserProfile(up);
            if (userProfile != null)
            {
                response.Token = security.GenerateToken(user.Email);
                response.Message = "Success";
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Please check your email and password.";
            }
            return response;
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

            byte[] pass = security.GetSHA1(user.Email, user.Password);
            UserProfile up = new UserProfile()
            {
                Email = user.Email,
                Password = pass,
                Nama = user.FirstName + " " + user.LastName,
                Telp = user.Telp,

                Created = DateTime.Now,
                CreatedBy = user.Email,
                RowStatus = true
            };

            long id = await dataAccess.Save(up);
            if (id > 0)
            {
                response.Token = security.GenerateToken(user.Email);
                response.Message = "Success created user profile.";
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Failed when save userprofile.";
            }
            return response;
        }

    }
}
