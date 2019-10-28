using ljgb.Infra.Jwt;
using ljgb.Infra.Session;
using System.Web.Http;


namespace ljgb.Infra.WebApi
{
    [JWTAuthenticationFilter]

 
    public class BaseApiController : ApiController
    {
        //public Logger appLogger = LogManager.GetCurrentClassLogger();
        public UserSession userSession;
    }
}
