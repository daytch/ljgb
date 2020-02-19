using ljgb.BusinessLogic;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ljgb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CMSController : Controller
    {

        private CMSFacade facade = new CMSFacade();
        private Security sec = new Security();
        [HttpGet]
        [Route("GetAllBanner")]
        public async Task<IActionResult> GetAllBanner()
        {
            try
            {
                var models = await facade.GetAllBanner();
                if (models == null)
                {
                    return NotFound();
                }

                return Ok(models);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetAllMaster")]
        public async Task<IActionResult> GetAllMaster()
        {
            try
            {
                var models = await facade.GetAllMaster();
                if (models == null)
                {
                    return NotFound();
                }

                return Ok(models);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("SubmitCMSChange")]
        public async Task<CMSResponse> SubmitCMSChange([FromBody]CMSRequest model)
        {
            CMSResponse resp = new CMSResponse();
            try
            {
                string bearer = Request.HttpContext.Request.Headers["Authorization"];
                string token = bearer.Substring("Bearer ".Length).Trim();
                string username = string.Empty;
                if (string.IsNullOrEmpty(token))
                {
                    resp.IsSuccess = false;
                    resp.Message = "You don't have access.";
                    return resp;
                }

                username = sec.ValidateToken(token);
                if (username == null)
                {
                    Response.HttpContext.Response.Cookies.Append("access_token", "", new CookieOptions()
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    });
                    resp.IsSuccess = false;
                    resp.Message = "Your session was expired, please re-login.";
                    return resp;
                }
                model.Username = username;

                resp = await facade.Submit(model);


                return resp;
            }
            catch (Exception)
            {
                return resp;
            }


        }

    

    }
}