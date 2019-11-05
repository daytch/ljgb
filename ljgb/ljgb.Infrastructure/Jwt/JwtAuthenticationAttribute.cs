using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ljgb.Infrastructure.Jwt
{
    public class JWTAuthenticationFilter : AuthorizationFilterAttribute
    {
        //private static string tokenBearer = "Bearer ";
        //protected NLog.Logger appLogger = NLog.LogManager.GetCurrentClassLogger();

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!IsUserAuthorized(actionContext))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            else
            {
                var controllerRef = actionContext.ControllerContext.Controller as WebApi.BaseApiController;
                controllerRef.userSession = Session.SessionManager.UserSession;
                //controllerRef.dataContext = new DataAccess.DataContext(controllerRef.userSession.AppUserId);
                base.OnAuthorization(actionContext);
            }

        }

        bool IsUserAuthorized(HttpActionContext actionContext)
        {

            var result = false;
            try
            {
                var authHeader = FetchFromHeader(actionContext);
                /*check bearer token query string add => || !authHeader.ToLower().StartsWith("bearer") */

                if (string.IsNullOrEmpty(authHeader))
                {
                    authHeader = HttpContext.Current.Request.QueryString["x-Token"].ToString();
                }

                if (string.IsNullOrEmpty(authHeader)) throw new ArgumentNullException("Invalid Token");

                var token = authHeader; //if use manual bearer  authHeader.Substring(7);

                var principal = AuthenticateJwtToken(token);
                if (principal == null) return false;


                return true;

            }
            catch (Exception ex)
            {
                result = false;
                //appLogger.Error(ex);
            }
            return result;

        }

        protected IPrincipal AuthenticateJwtToken(string token)
        {
            string username;

            if (ValidateToken(token, out username))
            {
                // based on username to get more information from database in order to build local identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Nama, username)
                    // Add more claims if needed: Roles, ...
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return user;
            }

            return null;
        }


        private static bool ValidateToken(string token, out string username)
        {
            username = null;
            var simplePrinciple = JwtManager.GetPrincipal(token);
            var identity = simplePrinciple?.Identity as ClaimsIdentity;
            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Nama);
            username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return false;

            // More validate to check whether username exists in system

            return true;
        }
        private string FetchFromHeader(HttpActionContext actionContext)
        {
            string requestToken = null;

            var authRequest = actionContext.Request.Headers.Authorization;


            if (authRequest != null)
            {
                requestToken = authRequest.Parameter;
            }

            return requestToken;
        }

    }
}
