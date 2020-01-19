using ljgb.BusinessLogic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Threading.Tasks;

namespace ljgb.API.Core
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate next;
        private static bool IsProduction;
        private readonly AuthenticationFacade facade;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AuthenticationMiddleware(RequestDelegate next)//,UserManager<IdentityUser> _userManager, IEmailSender _emailSender, SignInManager<IdentityUser> _signInManager)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            IsProduction = Convert.ToBoolean(configuration.GetSection("IsProduction").Value.ToString());
            facade = new AuthenticationFacade();
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string authoriztionHeader = context.Request.Headers["Authorization"];
            string url = MyHttpContext.AppBaseUrl;
            string requester_url = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request);
            log.Debug("API url = " + url + ", requerster url = " + requester_url);

            if (IsProduction)
            {
                #region Production Setting
                if (authoriztionHeader != null && authoriztionHeader.StartsWith("Bearer"))
                {
                    Security sec = new Security();
                    string token = authoriztionHeader.Substring("Bearer ".Length).Trim();

                    JwtSecurityTokenHandler jwthandler = new JwtSecurityTokenHandler();
                    Microsoft.IdentityModel.Tokens.SecurityToken jwttoken = jwthandler.ReadToken(token);
                    var expDate = jwttoken.ValidTo;

                    string username = sec.ValidateToken(token);
                    bool IsUserActive = facade.IsUserActive(username);

                    if (IsUserActive && expDate < DateTime.UtcNow.AddMinutes(1))
                    {
                        await next.Invoke(context);
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                }
                #endregion
            }
            else
            {
                #region Development Setting
                if (requester_url.ToLower().Contains(url.ToLower()) || requester_url.ToLower().Contains("swagger") || requester_url.ToLower().Contains("auth"))
                {

                    log.Debug("Lolos pada if development, (requester_url.ToLower().Contains(url.ToLower()) || requester_url.ToLower().Contains('swagger') || requester_url.ToLower().Contains('auth')");
                    await next.Invoke(context);
                }
                else
                {
                    if (authoriztionHeader != null && authoriztionHeader.StartsWith("Bearer"))
                    {
                        Security sec = new Security();
                        string token = authoriztionHeader.Substring("Bearer ".Length).Trim();

                        JwtSecurityTokenHandler jwthandler = new JwtSecurityTokenHandler();
                        Microsoft.IdentityModel.Tokens.SecurityToken jwttoken = jwthandler.ReadToken(token);
                        var expDate = jwttoken.ValidTo;

                        string username = sec.ValidateToken(token);
                        bool IsUserActive = facade.IsUserActive(username);

                        if (IsUserActive && expDate > DateTime.UtcNow.AddMinutes(1))
                        {
                            await next.Invoke(context);
                        }
                        else
                        {
                            context.Response.StatusCode = 401;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                    }
                }
                #endregion
            }
        }
    }

    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyCustomAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
