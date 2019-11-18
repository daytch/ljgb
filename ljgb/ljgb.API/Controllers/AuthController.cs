using ljgb.BusinessLogic;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ljgb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AuthenticationFacade facade = new AuthenticationFacade();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        [HttpPost]
        [Route("Login")]
        public async Task<AuthenticationResponse> Login([FromBody] UserRequest userInfo)
        {
            AuthenticationResponse resp = new AuthenticationResponse();
            try
            {
                resp = await facade.Login(userInfo);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                resp.IsSuccess = false;
                resp.Message = ex.Message;
            }

            return resp;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<AuthenticationResponse> Register([FromBody] UserRequest userInfo)
        {
            AuthenticationResponse resp = new AuthenticationResponse();
            try
            {
                resp = await facade.Register(userInfo);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                resp.IsSuccess = false;
                resp.Message = ex.Message;
            }

            return resp;
        }
    }
}