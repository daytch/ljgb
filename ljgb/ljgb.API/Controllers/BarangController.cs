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
    public class BarangController : ControllerBase
    {
        private BarangFacade facade = new BarangFacade();
        private string url = "";
        private Security sec = new Security();
        public BarangController(IConfiguration config)
        {
            url = config.GetSection("API_url").Value;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                string search = HttpContext.Request.Query["search[value]"].ToString();
                int draw = Convert.ToInt32(HttpContext.Request.Query["draw"]);
                string order = HttpContext.Request.Query["order[0][column]"];
                string orderDir = HttpContext.Request.Query["order[0][dir]"];
                int startRec = Convert.ToInt32(HttpContext.Request.Query["start"]);
                int pageSize = Convert.ToInt32(HttpContext.Request.Query["length"]);
                var models = await facade.GetAll(search, order, orderDir, startRec, pageSize, draw);
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
        [Route("GetAllForHomePage")]
        public IActionResult GetAllForHomePage([FromQuery]string city)
        {
            BarangResponse respon = new BarangResponse();
            try
            {
                respon = facade.GetAllForHomePage(city);
                respon.IsSuccess = true;
                respon.Message = "Success";

                return Ok(respon);
            }
            catch (Exception ex)
            {
                respon.Message = ex.Message;
                respon.IsSuccess = false;
                return BadRequest(respon);
            }
        }

        [HttpGet]
        [Route("GetBarangDetail")]
        public async Task<IActionResult> GetBarangDetail(int Id)
        {
            if (Id < 1)
            {
                return BadRequest();
            }

            try
            {
                BarangResponse post = await facade.GetBarangDetail(Id);
                post.IsSuccess = true;
                post.Message = "Success";

                if (post == null)
                {
                    return NotFound();
                }

                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetBidPosition")]
        public async Task<IActionResult> GetBidPosition(int Id, int Nominal)
        {
            if (Id < 1)
            {
                return BadRequest();
            }

            try
            {
                Position post = await facade.GetBidPosition(Id, Nominal);

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

        [HttpGet]
        [Route("GetAskPosition")]
        public async Task<IActionResult> GetAskPosition(int Id, int Nominal)
        {
            if (Id < 1)
            {
                return BadRequest();
            }

            try
            {
                Position post = await facade.GetAskPosition(Id, Nominal);

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

        [HttpGet]
        [Route("GetAllAsksById")]
        public IActionResult GetAllAsksById([FromQuery]BarangRequest request)
        {
            long Id = request.ID;//Convert.ToInt32(HttpContext.Request.Query["id"]);
            int start = Convert.ToInt32(HttpContext.Request.Query["start"]);
            int limit = Convert.ToInt32(HttpContext.Request.Query["limit"]);
            int max = Convert.ToInt32(HttpContext.Request.Query["max"]);

            if (Id < 1)
            {
                return BadRequest();
            }

            try
            {
                BarangResponse post = facade.GetAllAsksById(request);
                post.IsSuccess = true;
                post.Message = "Success";
                if (post == null)
                {
                    return NotFound();
                }

                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAllBidsById")]
        public IActionResult GetAllBidsById([FromQuery]BarangRequest request)
        {
            long Id = request.ID;//Convert.ToInt32(HttpContext.Request.Query["id"]);
            int start = Convert.ToInt32(HttpContext.Request.Query["start"]);
            int limit = Convert.ToInt32(HttpContext.Request.Query["limit"]);
            int max = Convert.ToInt32(HttpContext.Request.Query["max"]);

            if (Id < 1)
            {
                return BadRequest();
            }

            try
            {
                BarangResponse post = facade.GetAllBidsById(request);
                post.IsSuccess = true;
                post.Message = "Success";
                if (post == null)
                {
                    return NotFound();
                }

                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("GetModelWithID")]
        public async Task<IActionResult> GetPost(long postId)
        {
            if (postId < 1)
            {
                return BadRequest();
            }

            try
            {
                var post = await facade.GetPost(postId);

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
        public async Task<BarangResponse> AddPost([FromBody]BarangRequest model)
        {
            BarangResponse resp = new BarangResponse();
            try
            {
                string bearer = Request.HttpContext.Request.Headers["Authorization"];
                string token = bearer.Substring("Bearer ".Length).Trim();
                string username = string.Empty;
                if (string.IsNullOrEmpty(token))
                {
<<<<<<< HEAD
                    resp.IsSuccess = false;
                    resp.Message = "You don't have access.";
                    return resp;
                }

                username = sec.ValidateToken(token);
                if (username == null)
                {
                    Response.HttpContext.Response.Cookies.Append("access_token", "", new CookieOptions()
=======
                    string bearer = Request.HttpContext.Request.Headers["Authorization"];
                    string token = bearer.Substring("Bearer ".Length).Trim();
                    string username = string.Empty;
                    if (string.IsNullOrEmpty(token))
                    {
                        result.IsSuccess = false;
                        result.Message = "You don't have access.";
                        return BadRequest(result);
                    }

                    username = sec.ValidateToken(token);
                    if (username == null)
                    {
                        Response.HttpContext.Response.Cookies.Append("access_token", "", new CookieOptions()
                        {
                            Expires = DateTime.Now.AddDays(-1)
                        });
                        result.IsSuccess = false;
                        result.Message = "Your session was expired, please re-login.";
                        return BadRequest(result);
                    }
                    if (model.ID > 0)
                    {
                        result = await facade.UpdatePost(model);
                    }
                    else
>>>>>>> 2950fc3cbded93d7b2910d60578be62593369a4d
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    });
                    resp.IsSuccess = false;
                    resp.Message = "Your session was expired, please re-login.";
                    return resp;
                }
                model.UserName = username;
                if (model.ID > 0)
                {
                    resp = await facade.UpdatePost(model);
                }
                else
                {
                    resp = await facade.AddPost(model);
                }

                return resp;
            }
            catch (Exception)
            {
                return resp;
            }
            //BarangResponse result = new BarangResponse();
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        if (model.ID > 0)
            //        {
            //            result = await facade.UpdatePost(model);
            //        }
            //        else
            //        {
            //            result = await facade.AddPost(model);
            //        }
            //        return Ok(result);
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
        public async Task<BarangResponse> DeletePost(BarangRequest request)
        {
<<<<<<< HEAD
            BarangResponse resp = new BarangResponse();
=======
            BarangResponse response = new BarangResponse();

>>>>>>> 2950fc3cbded93d7b2910d60578be62593369a4d
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

<<<<<<< HEAD
=======
                string bearer = Request.HttpContext.Request.Headers["Authorization"];
                string token = bearer.Substring("Bearer ".Length).Trim();
                string username = string.Empty;
                if (string.IsNullOrEmpty(token))
                {
                    response.IsSuccess = false;
                    response.Message = "You don't have access.";
                    return BadRequest(response);
                }

                username = sec.ValidateToken(token);
                if (username == null)
                {
                    Response.HttpContext.Response.Cookies.Append("access_token", "", new CookieOptions()
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    });
                    response.IsSuccess = false;
                    response.Message = "Your session was expired, please re-login.";
                    return BadRequest(response);
                }
                response = await facade.DeletePost(request.ID);
>>>>>>> 2950fc3cbded93d7b2910d60578be62593369a4d


                return resp = await facade.DeletePost(request.ID, username); ;
            }
            catch (Exception)
            {
                return resp;
            }
        }
        //    try
        //    {
        //        if (request.ID < 1)
        //        {
        //            return BadRequest();
        //        }

        //        response = await facade.DeletePost(request.ID);

        //        return Ok(response);
        //    }
        //    catch (Exception)
        //    {

        //        return BadRequest();
        //    }
        //}

        [HttpPost]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody]BarangRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BarangResponse response = new BarangResponse();
                    string bearer = Request.HttpContext.Request.Headers["Authorization"];
                    string token = bearer.Substring("Bearer ".Length).Trim();
                    string username = string.Empty;
                    if (string.IsNullOrEmpty(token))
                    {
                        response.IsSuccess = false;
                        response.Message = "You don't have access.";
                        return BadRequest(response);
                    }

                    username = sec.ValidateToken(token);
                    if (username == null)
                    {
                        Response.HttpContext.Response.Cookies.Append("access_token", "", new CookieOptions()
                        {
                            Expires = DateTime.Now.AddDays(-1)
                        });
                        response.IsSuccess = false;
                        response.Message = "Your session was expired, please re-login.";
                        return BadRequest(response);
                    }
                    var result = await facade.UpdatePost(request);

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
        [Route("GetHargaOTR")]
        public async Task<IActionResult> GetHargaOTR([FromBody]BarangRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await facade.GetHargaOTR(request);

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
        [Route("UploadFile")]
        public async Task<string> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "Please select profile picture";

            string folderName = Path.Combine("Resources", "UploadDocs");
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + file.FileName;
            string dbPath = Path.Combine(folderName, uniqueFileName);

            using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }

        [HttpPost]
        [Route("SubmitUpload")]
        public async Task<BarangResponse> SubmitUpload(string fileName)
        {
            BarangResponse resp = new BarangResponse();
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
                resp = await facade.SubmitUpload(fileName, username);

                return resp;
            }
            catch (Exception)
            {
                return resp;
            }
        }


        [HttpPost]
        [Route("UploadImageBarang")]
        public async Task<string> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "Please select profile picture";

            string folderName = Path.Combine("Resources", "UploadImageBarang");
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + file.FileName;
            string dbPath = Path.Combine(folderName, uniqueFileName);

            using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return url + dbPath;
        }

        [HttpPost]
        [Route("GetBarangByHomeParameter")]
        public async Task<IActionResult> GetBarangByHomeParameter([FromBody]BarangRequest request)
        {
            BarangResponse result = new BarangResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    result = await facade.GetBarangByHomeParameter(request);
                    return Ok(result);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("GetPhotoAndWarnaByID")]
        public async Task<IActionResult> GetPhotoAndWarnaByID([FromBody]BarangRequest request)

        {
            BarangResponse result = new BarangResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    result = await facade.GetPhotoAndWarnaByID(request);
                    return Ok(result);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

    }
}
