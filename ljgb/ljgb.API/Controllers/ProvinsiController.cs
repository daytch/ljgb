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
    public class ProvinsiController : ControllerBase
    {
        private ProvinsiFacade facade = new ProvinsiFacade();
        private Security sec = new Security();
        [HttpGet]
        [Route("GetAll")]
        public async Task<ProvinsiResponse> GetAll()
        {

            ProvinsiResponse resp = new ProvinsiResponse();
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
                int draw = Convert.ToInt32(HttpContext.Request.Query["draw"]);
                string order = HttpContext.Request.Query["order[0][column]"];
                string orderDir = HttpContext.Request.Query["order[0][dir]"];
                int startRec = Convert.ToInt32(HttpContext.Request.Query["start"]);
                int pageSize = Convert.ToInt32(HttpContext.Request.Query["length"]);
                 resp = await facade.GetAll(search, order, orderDir, startRec, pageSize, draw);
               

                return resp;
            }
            catch (Exception ex)
            {
                resp.IsSuccess = false;
                resp.Message = ex.Message.ToString();
                return resp;
            }
            //try
            //{
            //    string search = HttpContext.Request.Query["search[value]"].ToString();
            //    int draw = Convert.ToInt32(HttpContext.Request.Query["draw"]);
            //    string order = HttpContext.Request.Query["order[0][column]"];
            //    string orderDir = HttpContext.Request.Query["order[0][dir]"];
            //    int startRec = Convert.ToInt32(HttpContext.Request.Query["start"]);
            //    int pageSize = Convert.ToInt32(HttpContext.Request.Query["length"]);
            //    var models = await facade.GetAll(search, order, orderDir, startRec, pageSize, draw);
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

        [HttpGet]
        [Route("GetAllForDropdown")]
        public async Task<IActionResult> GetAllForDropdown()
        {
            ProvinsiResponse response = new ProvinsiResponse();
            try
            {
                response.ListProvinces = await facade.GetAllForDropdown();
                response.IsSuccess = true;
                response.Message = "Success";

                if (response == null)
                {
                    return NotFound();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
       
        //[HttpGet]
        //[Route("GetAllWithoutFilter")]
        //public async Task<IActionResult> GetAllWithoutFilter()
        //{
        //    try
        //    {

        //        var models = await facade.GetAllWithoutFilter();
        //        if (models == null)
        //        {
        //            return NotFound();
        //        }

        //        return Ok(models);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        [HttpPost]
        [Route("GetModelWithID")]
        public async Task<IActionResult> GetPost(ProvinsiRequest req)
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
        [Route("AddPost")]
        public async Task<ProvinsiResponse> AddPost([FromBody]ProvinsiRequest req)
        {
            ProvinsiResponse resp = new ProvinsiResponse();
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
                req.UserName = username;
                if (req.ID > 0)
                {
                    resp = await facade.UpdatePost(req);
                }
                else
                {
                    resp = await facade.AddPost(req);
                }



                return resp;
            }
            catch (Exception ex)
            {
                resp.IsSuccess = false;
                resp.Message = ex.Message.ToString();
                return resp;
            }
            //ProvinsiResponse response = new ProvinsiResponse();
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        if (req.ID >0)
            //        {
            //            response = await facade.UpdatePost(req);
            //        }
            //        else
            //        {
            //            response = await facade.AddPost(req);
            //        }

            //        return Ok(response);

            //    }
            //    catch (Exception)
            //    {
            //        return BadRequest();
            //    }

            //}

            //return BadRequest();
        }

        [HttpPost]
        [Route("DeletePost")]
        public async Task<ProvinsiResponse> DeletePost(ProvinsiRequest req)
        {
            ProvinsiResponse resp = new ProvinsiResponse();
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
                req.UserName = username;
            


                return resp = await facade.DeletePost(req);
            }
            catch (Exception ex)
            {
                resp.IsSuccess = false;
                resp.Message = ex.Message.ToString();
                return resp;
            }
            //try
            //{                
            //    var result = await facade.DeletePost(req);

            //    return Ok(result);
            //}
            //catch (Exception)
            //{
            //    return BadRequest();
            //}
        }

        [HttpPost]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody]ProvinsiRequest req)
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
    }
}
