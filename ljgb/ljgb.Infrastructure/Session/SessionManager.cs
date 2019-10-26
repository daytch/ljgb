using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;

namespace ljgb.Infrastructure.Session
{
    public class SessionManager
    {
        public static UserSession FromToken(string token)
        {
            var tokenClaimData = JwtManager.GetPrincipal(token);
            var tokenValue = tokenClaimData?.Claims?.ToList()?[1]?.Value;
            return JsonConvert.DeserializeObject<UserSession>(tokenValue);
        }

        public static UserSession UserSession
        {
            get
            {
                var serverType = HttpContext.Current.Request.Headers["Server-Type"];
                //if (serverType.Equals(AppConstants.ServerTypeAPI) || string.IsNullOrEmpty(serverType)) throw new Exception("Server type is required!");

                if (string.IsNullOrEmpty(serverType))
                {
                    if (HttpContext.Current.Request.QueryString["x-Token"] != null)
                    {
                        serverType = "X-TOKEN";
                    }
                }

                if (serverType == AppConstants.ServerTypeAPI)
                {
                    try
                    {
                        var token = HttpContext.Current.Request.Headers["Authorization"];
                        token = token.Substring(7);
                        var tokenClaimData = JwtManager.GetPrincipal(token);
                        var tokenValue = tokenClaimData?.Claims?.ToList()?[1]?.Value;
                        return JsonConvert.DeserializeObject<UserSession>(tokenValue);
                    }
                    catch (Exception ex)
                    {
                        //appLogger.Error(ex);
                        return null;
                    }
                }
                else if (serverType == "X-TOKEN")
                {
                    try
                    {
                        var token = HttpContext.Current.Request.QueryString["x-Token"].ToString();
                        var tokenClaimData = JwtManager.GetPrincipal(token);
                        var tokenValue = tokenClaimData?.Claims?.ToList()?[1]?.Value;
                        return JsonConvert.DeserializeObject<UserSession>(tokenValue);
                    }
                    catch (Exception ex)
                    {
                        //appLogger.Error(ex);
                        return null;
                    }
                }
                else
                {
                    return (UserSession)HttpContext.Current.Session[AppConstants.SessionKey];
                }
            }
        }

    }
}
