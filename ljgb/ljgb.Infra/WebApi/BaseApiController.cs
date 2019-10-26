using ljgb.Infra.Jwt;
using ljgb.Infra.Session;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ljgb.Infra.WebApi
{
    [JWTAuthenticationFilter]

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BaseApiController : ApiController
    {
        //public Logger appLogger = LogManager.GetCurrentClassLogger();
        public UserSession userSession;
    }
}
