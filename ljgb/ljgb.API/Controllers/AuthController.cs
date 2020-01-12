using ljgb.API.Helper;
using ljgb.BusinessLogic;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ljgb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //private readonly UserManager<IdentityUser> userManager;
        //private readonly IEmailSender emailSender;
        //private readonly SignInManager<IdentityUser> signInManager;
        private ConfigFacade configFacade = new ConfigFacade();
        private readonly AuthenticationFacade facade;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IEmailConfiguration _emailConfiguration;
        private string admin_url = string.Empty;
        private string ui_url = string.Empty;
        private Security sec = new Security();
        public AuthController(IConfiguration config)//(IEmailConfiguration EmailConfiguration, UserManager<IdentityUser> _userManager, IEmailSender _emailSender, SignInManager<IdentityUser> _signInManager)
        {
            //userManager = _userManager;
            //emailSender = _emailSender;
            //signInManager = _signInManager;
            //_emailConfiguration = EmailConfiguration;
            facade = new AuthenticationFacade();// (userManager, emailSender, signInManager);
            admin_url = config.GetSection("ADMIN_url").Value;
            ui_url = config.GetSection("UI_url").Value;
        }

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
                if (resp.IsSuccess)
                {
                    #region Sent Email to User
                    SendEmail sendEmail = new SendEmail(_emailConfiguration);
                    string contentEmail = configFacade.GetRedaksionalEmail("ContentEmailRegistration").Result;
                    string subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailRegistration").Result;

                    EmailAddress emailAddress = new EmailAddress();
                    emailAddress.Address = userInfo.Email;
                    emailAddress.Name = userInfo.FirstName + " " + userInfo.LastName;
                    List<EmailAddress> listEmailAddress = new List<EmailAddress>();
                    listEmailAddress.Add(emailAddress);

                    contentEmail = contentEmail.Replace("[user]", emailAddress.Name);

                    EmailAddress emailAddressFrom = new EmailAddress();
                    emailAddressFrom.Address = "admin@lojualguebeli.com";
                    emailAddressFrom.Name = "Lojualguebeli.com";
                    List<EmailAddress> listEmailAddressFrom = new List<EmailAddress>();
                    listEmailAddressFrom.Add(emailAddressFrom);

                    EmailMessage emailMessage = new EmailMessage();
                    emailMessage.ToAddresses = listEmailAddress;
                    emailMessage.Subject = subjectEmail;
                    emailMessage.FromAddresses = listEmailAddressFrom;
                    emailMessage.Content = contentEmail;

                    sendEmail.Send(emailMessage);
                    #endregion
                }
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
        [Route("ForgotPassword")]
        public async Task<AuthenticationResponse> ForgotPassword([FromBody] UserRequest userInfo)
        {
            AuthenticationResponse resp = new AuthenticationResponse();
            try
            {
                resp = await facade.ForgotPassword(userInfo);
                if (resp.IsSuccess)
                {
                    #region Sent Email to User
                    SendEmail sendEmail = new SendEmail(_emailConfiguration);
                    string contentEmail = configFacade.GetRedaksionalEmail("ContentEmailForgotPassword").Result;
                    string subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailForgotPassword").Result;

                    EmailAddress emailAddress = new EmailAddress();
                    emailAddress.Address = userInfo.Email;
                    emailAddress.Name = resp.Name;
                    List<EmailAddress> listEmailAddress = new List<EmailAddress>();
                    listEmailAddress.Add(emailAddress);

                    string token = facade.GenerateToken(userInfo.Email);
                    string url = ui_url.Contains(Request.Host.Value) ? ui_url + "resetpassword?token=" + token
                        : admin_url + "identity/account/resetpassword?token=" + token;
                    contentEmail = contentEmail.Replace("[user]", emailAddress.Name);
                    contentEmail = contentEmail.Replace("[url]", url);

                    EmailAddress emailAddressFrom = new EmailAddress();
                    emailAddressFrom.Address = "admin@lojualguebeli.com";
                    emailAddressFrom.Name = "Lojualguebeli.com";
                    List<EmailAddress> listEmailAddressFrom = new List<EmailAddress>();
                    listEmailAddressFrom.Add(emailAddressFrom);

                    EmailMessage emailMessage = new EmailMessage();
                    emailMessage.ToAddresses = listEmailAddress;
                    emailMessage.Subject = subjectEmail;
                    emailMessage.FromAddresses = listEmailAddressFrom;
                    emailMessage.Content = contentEmail;

                    sendEmail.Send(emailMessage);
                    #endregion
                }
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
        [Route("ChangePassword")]
        public async Task<AuthenticationResponse> ChangePassword([FromBody] UserRequest userInfo)
        {
            AuthenticationResponse resp = new AuthenticationResponse();
            try
            {
                string token = userInfo.Token;
                string username = string.Empty;
                if (string.IsNullOrEmpty(token))
                {
                    resp.IsSuccess = false;
                    resp.Message = "Your token is invalid or has been expired.";
                    return resp;
                }

                userInfo.Email = sec.ValidateToken(token);
                if (string.IsNullOrEmpty(userInfo.Email))
                {
                    resp.IsSuccess = false;
                    resp.Message = "Your token is invalid or has been expired.";
                    return resp;
                }

                if (await facade.UpdatePassword(userInfo))
                {
                    resp.IsSuccess = true;
                    resp.Message = "Success, Password Updated.";
                }
                else
                {
                    resp.IsSuccess = false;
                    resp.Message = "Failed to update password";
                    return resp;
                }
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