﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ljgb.BusinessLogic;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Model;
using Microsoft.AspNetCore.Authentication;
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

        [HttpPost]
        [Route("GetSalesman")]
        public async Task<IActionResult> GetAllSalesman()
        {
            try
            {
                var salesman = await facade.GetAllSalesman();
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

        [HttpPut]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody]UserProfile model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await facade.UpdatePost(model);

                    return Ok();
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

        [HttpPost]
        [Route("PasswordSignIn")]
        public async Task<SignInResponse> PasswordSignIn([FromBody]UserRequest user)
        {
            SignInResponse result = new SignInResponse();
            try
            {
                //Microsoft.AspNetCore.Identity.SignInResult res = await facade.PasswordSignIn(model);
                var res = await signInManager.PasswordSignInAsync(user.user.Email, user.password, user.RememberMe, user.lockoutOnFailure);
                result = new SignInResponse()
                {
                    Succeeded = res.Succeeded,
                    IsLockedOut = res.IsLockedOut,
                    IsNotAllowed = res.RequiresTwoFactor
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}
