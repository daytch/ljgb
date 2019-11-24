using ljgb.API.Helper;
using ljgb.BusinessLogic;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ljgb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ConfigFacade configFacade = new ConfigFacade();
        private AuthenticationFacade facade = new AuthenticationFacade();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IEmailConfiguration _emailConfiguration;
        public AuthController(IEmailConfiguration EmailConfiguration)
        {
            _emailConfiguration = EmailConfiguration;
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
    }
}