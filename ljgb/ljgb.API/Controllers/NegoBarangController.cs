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
    public class NegoBarangController : ControllerBase
    {
        private NegoBarangFacade facade = new NegoBarangFacade();
        private Security sec = new Security();
        [HttpPost]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var models = await facade.GetAll();
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
        [Route("GetModelWithID")]
        public async Task<IActionResult> GetPost(NegoBarangRequest req)
        {
            if (req == null)
            {
                return BadRequest();
            }

            try
            {
                var post = await facade.GetPost(req);

                if (post == null)
                {
                    return NotFound();
                }

                return Ok(post);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("SubmitBid")]
        public async Task<NegoBarangResponse> SubmitBid([FromBody]NegoBarangRequest request)
        {

            NegoBarangResponse resp = new NegoBarangResponse();
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
                request.UserName = username;
                resp = await facade.SubmitBid(request);
                return resp;
            }
            catch (Exception)
            {
                return resp;
            }
          
        }


        [HttpPost]
        [Route("SubmitAsk")]
        public async Task<NegoBarangResponse> SubmitAsk([FromBody]NegoBarangRequest request)
        {
            NegoBarangResponse resp = new NegoBarangResponse();
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
                request.UserName = username;
                resp = await facade.Submitask(request);
                return resp;
            }
            catch (Exception)
            {
                return resp;
            }
            //NegoBarangResponse response = new NegoBarangResponse();
            //try
            //{
            //    response = await facade.Submitask(request);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex);
            //}
            //return Ok(response);
        }


        [HttpPost]
        [Route("DeletePost")]
        public async Task<IActionResult> DeletePost(NegoBarangRequest req)
        {

            try
            {
                var result = await facade.DeletePost(req);

                return Ok(result);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


        [HttpPost]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody]NegoBarangRequest req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await facade.UpdatePost(req);

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    if (ex.GetType().FullName ==
                             "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                    {
                        return NotFound();
                    }

                    return BadRequest();
                }
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("GetAllASK")]
        public async Task<NegoBarangResponse> GetAllASK([FromBody]DTParameters param)
        {
            NegoBarangResponse resp = new NegoBarangResponse();
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
                string search = HttpContext.Request.Query["search[value]"].ToString();
                int draw = param.Draw;
                string order = param.Order[0].Column.ToString();
                string orderDir = param.Order[0].Dir.ToString();
                int startRec = param.Start;
                int pageSize = param.Length;
                resp = await facade.GetAllASK(search, order, orderDir, startRec, pageSize, draw, username);
        

                return resp;
            }
            catch (Exception)
            {
                return resp;
            }
         
        }

        [HttpPost]
        [Route("GetAllBID")]
        public async Task<NegoBarangResponse> GetAllBID([FromBody]DTParameters param)
        {
            NegoBarangResponse resp = new NegoBarangResponse();
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
                string search = HttpContext.Request.Query["search[value]"].ToString();
                int draw = param.Draw;
                string order = param.Order[0].Column.ToString();
                string orderDir = param.Order[0].Dir.ToString();
                int startRec = param.Start;
                int pageSize = param.Length;
                resp = await facade.GetAllBID(search, order, orderDir, startRec, pageSize, draw, username);


                return resp;
            }
            catch (Exception)
            {
                return resp;
            }
            //try
            //{


            //    //var test1 = HttpContext.Request.Form;

            //    string search = HttpContext.Request.Query["search[value]"].ToString();
            //    int draw = param.Draw;
            //    string order = param.Order[0].Column.ToString();
            //    string orderDir = param.Order[0].Dir.ToString();
            //    int startRec = param.Start;
            //    int pageSize = param.Length;
            //    var models = await facade.GetAllBID(search, order, orderDir, startRec, pageSize, draw);
            //    if (models == null)
            //    {
            //        return NotFound();
            //    }

            //    return Ok(models);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex);
            //}
        }
    }
}
