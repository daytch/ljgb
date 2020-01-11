using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ljgb.BusinessLogic;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ljgb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly UserFacade facade;
        private readonly IEmailSender emailSender;
        private readonly SignInManager<IdentityUser> signInManager;
        private Security sec = new Security();
        private string url = string.Empty;

        public UserController(UserManager<IdentityUser> _userManager, IEmailSender _emailSender, SignInManager<IdentityUser> _signInManager, IConfiguration config)
        {
            userManager = _userManager;
            emailSender = _emailSender;
            signInManager = _signInManager;
            facade = new UserFacade(userManager, emailSender, signInManager);
            url = config.GetSection("API_url").Value;
        }

        [HttpPost]
        [Route("GetUser")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var categories = await facade.GetAllUser();
                if (categories == null)
                {
                    return NotFound();
                }

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetSalesman")]
        public async Task<IActionResult> GetAllSalesman()
        {
            try
            {
                string search = HttpContext.Request.Query["search[value]"].ToString();
                int draw = Convert.ToInt32(HttpContext.Request.Query["draw"]);
                string order = HttpContext.Request.Query["order[0][column]"];
                string orderDir = HttpContext.Request.Query["order[0][dir]"];
                int startRec = Convert.ToInt32(HttpContext.Request.Query["start"]);
                int pageSize = Convert.ToInt32(HttpContext.Request.Query["length"]);
                var salesman = await facade.GetAllSalesman(search, order, orderDir, startRec, pageSize, draw);
                if (salesman == null)
                {
                    return NotFound();
                }

                return Ok(salesman);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetSalesmanById")]
        public async Task<IActionResult> GetSalesmanById(int id)
        {
            try
            {
                var salesman = await facade.GetSalesmanById(id);
                if (salesman == null)
                {
                    return NotFound();
                }

                return Ok(salesman);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetBuyer")]
        public async Task<IActionResult> GetAllBuyer()
        {
            try
            {
                string search = HttpContext.Request.Query["search[value]"].ToString();
                int draw = Convert.ToInt32(HttpContext.Request.Query["draw"]);
                string order = HttpContext.Request.Query["order[0][column]"];
                string orderDir = HttpContext.Request.Query["order[0][dir]"];
                int startRec = Convert.ToInt32(HttpContext.Request.Query["start"]);
                int pageSize = Convert.ToInt32(HttpContext.Request.Query["length"]);
                var buyer = await facade.GetAllBuyer(search, order, orderDir, startRec, pageSize, draw);
                if (buyer == null)
                {
                    return NotFound();
                }

                return Ok(buyer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("GetPosts")]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                var posts = await facade.GetPosts();
                if (posts == null)
                {
                    return NotFound();
                }

                return Ok(posts);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("GetPost")]
        public async Task<UserResponse> GetPost([FromBody]UserRequest request)
        {
            string bearer = Request.HttpContext.Request.Headers["Authorization"];
            string token = bearer.Substring("Bearer ".Length).Trim();
            UserResponse resp = new UserResponse();
            string username = sec.ValidateToken(token);
            if (!string.IsNullOrEmpty(username))
            {
                try
                {
                    resp = await facade.GetPost(username);

                    if (resp == null)
                    {
                        resp.IsSuccess = false;
                        resp.Message = "Not Found";
                    }

                    return resp;
                }
                catch (Exception ex)
                {
                    resp.IsSuccess = false;
                    resp.Message = ex.Message.ToString();
                    return resp;
                }
            }
            else
            {

                    Response.HttpContext.Response.Cookies.Append("access_token", "", new CookieOptions()
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    });
                    resp.IsSuccess = false;
                    resp.Message = "Your session was expired, please re-login.";
                    return resp;
            }

        }

        [HttpPost]
        [Route("AddPost")]
        public async Task<IActionResult> AddPost([FromBody]UserProfile model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var postId = await facade.AddPost(model);
                    if (postId > 0)
                    {
                        return Ok(postId);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception)
                {
                    return BadRequest();
                }

            }

            return BadRequest();
        }

        [HttpPost]
        [Route("DeletePost")]
        public async Task<IActionResult> DeletePost(long postId)
        {
            long result = 0;

            if (postId < 1)
            {
                return BadRequest();
            }

            try
            {
                result = await facade.DeletePost(postId);
                if (result == 0)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        //[HttpPost]
        //[Route("Register")]
        //public async Task<RegisterResponse> Register([FromBody]UserRequest model)
        //{
        //    //return await userManager.CreateAsync(model.user, model.password);
        //    IdentityResult result = new IdentityResult();
        //    RegisterResponse resp = new RegisterResponse();
        //    try
        //    {
        //        result = await facade.Register(model);
        //        resp = new RegisterResponse()
        //        {
        //            Succeeded = result.Succeeded,
        //            Errors = result.Errors
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return resp;
        //    //return await facade.Register(model);
        //}

        [HttpPost]
        [Route("SaveSalesman")]
        public async Task<UserResponse> SaveSalesman([FromBody]UserRequest model)
        {
            UserResponse response = new UserResponse();
            try
            {
                string bearer = Request.HttpContext.Request.Headers["Authorization"];
                string token = bearer.Substring("Bearer ".Length).Trim();
                string username = string.Empty;
                if (string.IsNullOrEmpty(token))
                {
                    response.IsSuccess = false;
                    response.Message = "You don't have access.";
                    return response;
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
                    return response;
                }

                response = await facade.SaveSalesman(model,username);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        //[HttpPost]
        //[Route("GenerateEmailConfirmationToken")]
        //public async Task<string> GenerateEmailConfirmationToken([FromBody]UserRequest model)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        result = await facade.GenerateEmailConfirmationToken(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        //[HttpPost]
        //[Route("SendConfirmationEmail")]
        //public async Task<bool> SendConfirmationEmail([FromBody]UserRequest model)
        //{
        //    bool result = false;
        //    try
        //    {
        //        result = await facade.SendConfirmationEmail(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        //[HttpPost]
        //[Route("SignIn")]
        //public async Task<bool> SignIn([FromBody]UserRequest model)
        //{
        //    bool result = false;
        //    try
        //    {
        //        result = await facade.SignIn(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        //[HttpGet]
        //[Route("GetExternalAuthenticationSchemes")]
        //public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemes()
        //{
        //    IEnumerable<AuthenticationScheme> result;
        //    try
        //    {
        //        result = await facade.GetExternalAuthenticationSchemes();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        [HttpPost]
        [Route("UpdateProfileSalesman")]
        public async Task<UserResponse> UpdateProfileSalesman([FromBody]UserRequest req)
        {
            UserResponse resp = new UserResponse();
            try
            {

                string bearer = Request.HttpContext.Request.Headers["Authorization"];
                string token = bearer.Substring("Bearer ".Length).Trim();
                //string token = req.Token;
                string username = string.Empty;
                if (string.IsNullOrEmpty(token))
                {
                    resp.IsSuccess = false;
                    resp.Message = "You don't have access.";
                    return resp;
                }

                req.Email = sec.ValidateToken(token);
                if (string.IsNullOrEmpty(req.Email))
                {
                    resp.IsSuccess = false;
                    resp.Message = "Your Token Is Expired Please Relogin.!";
                    return resp;
                }
                resp = await facade.UpdateProfileSalesman(req);


                return resp;
            }
            catch (Exception ex)
            {
                resp.IsSuccess = false;
                resp.Message = ex.ToString();
                return resp;
            }
        }


        [HttpPost]
        [Route("UploadImageProfile")]
        public async Task<string> UploadImageProfile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "Please select profile picture";

            string folderName = Path.Combine("Resources", "UploadImageProfile");
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
        [Route("ChangePassword")] 
        public async Task<UserResponse> ChangePassword([FromBody]UserRequest req)
        {
            UserResponse resp = new UserResponse();
            try
            {
                string bearer = Request.HttpContext.Request.Headers["Authorization"];
                string token = bearer.Substring("Bearer ".Length).Trim();
                //string token = req.Token;
                string username = string.Empty;
                if (string.IsNullOrEmpty(token))
                {
                    resp.IsSuccess = false;
                    resp.Message = "You don't have access.";
                    return resp;
                }

                req.Email = sec.ValidateToken(token);
                if (string.IsNullOrEmpty(req.Email))
                {
                    resp.IsSuccess = false;
                    resp.Message = "Your Token Is Expired Please Relogin.!";
                    return resp;
                }
                if (await facade.UpdatePassword(req))
                {
                    resp.IsSuccess = true;
                    resp.Message = "Password Updated.!";
                }
                else
                {

                    resp.IsSuccess = false;
                    resp.Message = "Failed to update password";
                    return resp;
                }

                return resp;
            }
            catch (Exception ex)
            {
                resp.IsSuccess = false;
                resp.Message = ex.ToString();
                return resp;
            }
        }
    }
}
