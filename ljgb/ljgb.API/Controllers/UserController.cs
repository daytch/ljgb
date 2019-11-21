using System;
using System.Collections.Generic;
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
        private string access_token = "";
        public UserController(UserManager<IdentityUser> _userManager, IEmailSender _emailSender, SignInManager<IdentityUser> _signInManager)
        {
            userManager = _userManager;
            emailSender = _emailSender;
            signInManager = _signInManager;
            facade = new UserFacade(userManager, emailSender, signInManager);
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

        //[HttpPost]
        //[Route("SaveUserDetail")]
        //public async Task<IActionResult> SaveUserDetail([FromBody]UserRequest request)
        //{
        //    UserResponse resp = new UserResponse();
        //    try
        //    {
        //        bool posts = await facade.SaveUserDetail(request);
        //        resp.IsSuccess = posts;
        //        if (posts)
        //        {
        //            resp.Message = "Success When Update UserDetail.";
        //        }
        //        else
        //        {
        //            resp.Message = "Failed when Update UserDetail.";
        //        }

        //        return Ok(resp);
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.Message = ex.ToString();
        //        resp.IsSuccess = false;
        //    }
        //    return Ok(resp);
        //}

        //[HttpPost]
        //[Route("DeleteUserDetail")]
        //public async Task<IActionResult> DeleteUserDetail([FromBody]UserRequest request)
        //{
        //    UserResponse resp = new UserResponse();
        //    try
        //    {
        //        resp.IsSuccess = await facade.DeleteUserDetail(request);
        //        if (resp.IsSuccess)
        //        {
        //            resp.Message = "Success when deleted data.";
        //        }
        //        else
        //        {
        //            resp.Message = "Failed when deleted data.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.IsSuccess = false;
        //        resp.Message = ex.ToString();
        //    }
        //    return Ok(resp);
        //}

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
        public async Task<IActionResult> GetPost([FromBody]UserRequest request)
        {
            
            string username = sec.ValidateToken(request.Token);
            if (username != "")
            {
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

           
            return BadRequest();
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

        [HttpPost]
        [Route("Register")]
        public async Task<RegisterResponse> Register([FromBody]UserRequest model)
        {
            //return await userManager.CreateAsync(model.user, model.password);
            IdentityResult result = new IdentityResult();
            RegisterResponse resp = new RegisterResponse();
            try
            {
                result = await facade.Register(model);
                resp = new RegisterResponse()
                {
                    Succeeded = result.Succeeded,
                    Errors = result.Errors
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resp;
            //return await facade.Register(model);
        }

        [HttpPost]
        [Route("SaveSalesman")]
        public async Task<UserResponse> SaveSalesman([FromBody]UserRequest model)
        {
            UserResponse response = new UserResponse();
            try
            {
                response = await facade.SaveSalesman(model);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        [HttpPost]
        [Route("GenerateEmailConfirmationToken")]
        public async Task<string> GenerateEmailConfirmationToken([FromBody]UserRequest model)
        {
            string result = string.Empty;
            try
            {
                result = await facade.GenerateEmailConfirmationToken(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        [HttpPost]
        [Route("SendConfirmationEmail")]
        public async Task<bool> SendConfirmationEmail([FromBody]UserRequest model)
        {
            bool result = false;
            try
            {
                result = await facade.SendConfirmationEmail(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<bool> SignIn([FromBody]UserRequest model)
        {
            bool result = false;
            try
            {
                result = await facade.SignIn(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        [HttpGet]
        [Route("GetExternalAuthenticationSchemes")]
        public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemes()
        {
            IEnumerable<AuthenticationScheme> result;
            try
            {
                result = await facade.GetExternalAuthenticationSchemes();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        //[HttpPost]
        //[Route("PasswordSignIn")]
        //public async Task<SignInResponse> PasswordSignIn([FromBody]UserRequest user)
        //{
        //    SignInResponse result = new SignInResponse();
        //    try
        //    {
        //        //Microsoft.AspNetCore.Identity.SignInResult res = await facade.PasswordSignIn(model);
        //        var res = await signInManager.PasswordSignInAsync(user.user.Email, user.password, user.RememberMe, user.lockoutOnFailure);
        //        result = new SignInResponse()
        //        {
        //            Succeeded = res.Succeeded,
        //            IsLockedOut = res.IsLockedOut,
        //            IsNotAllowed = res.RequiresTwoFactor
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

    }
}
