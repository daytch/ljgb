using ljgb.Infrastructure.Jwt;
using ljgb.Infrastructure.Session;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ljgb.Infrastructure.WebApi
{
    [JWTAuthenticationFilter]

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BaseApiController : ApiController
    {
        //public Logger appLogger = LogManager.GetCurrentClassLogger();
        public UserSession userSession;
    }
}
