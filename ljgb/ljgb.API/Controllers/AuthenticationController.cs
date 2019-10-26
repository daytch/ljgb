using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ljgb.API.Core;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ljgb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        //private readonly UserManager<IdentityUser> userManager;
        //private readonly UserFacade facade;
        //private readonly IEmailSender emailSender;
        private readonly SignInManager<IdentityUser> signInManager;
        private IConfiguration configuration;
        private Authentication auth;
        public AuthenticationController(SignInManager<IdentityUser> _signInManager, IConfiguration _configuration)// UserManager<IdentityUser> _userManager, IEmailSender _emailSender, SignInManager<IdentityUser> _signInManager)
        {
            //userManager = _userManager;
            //emailSender = _emailSender;
            signInManager = _signInManager;
            configuration = _configuration;
            auth = new Authentication(_configuration);
            // facade = new UserFacade(userManager, emailSender, signInManager);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<AuthenticationResponse> Login([FromBody] UserRequest userInfo)
        {
            AuthenticationResponse resp = new AuthenticationResponse() { Succeeded = false };
            var result = await signInManager.PasswordSignInAsync(userInfo.user.Email, userInfo.password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                resp = BuildToken(userInfo);
                //ClaimsPrincipal prin = auth.GetPrincipal(resp.Token);
                resp.Succeeded = true;
                return resp;
            }
            //else
            //{
            //    //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            //    //return BadRequest(ModelState);
            //}
            return resp;
        }

        private AuthenticationResponse BuildToken(UserRequest user)
        {
            AuthenticationResponse response = new AuthenticationResponse();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.user.Email),
                new Claim("CariMobilMurah", "LoJualGueBeli.com aja"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Kata_Paling_Rahasia"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: configuration["Jwt:Issuer"],
               audience: configuration["Jwt:Issuer"],
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return response = new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expired = DateTime.UtcNow.AddHours(1)
            };
        }
    }
}