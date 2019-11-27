using ljgb.API.Helper;
using ljgb.BusinessLogic;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ljgb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly UserFacade usrFacade;
        private readonly IEmailSender emailSender;

        private readonly SignInManager<IdentityUser> signInManager;
        private string url = "";
        private Security sec = new Security();
        private ConfigFacade configFacade = new ConfigFacade();
        private IEmailConfiguration _emailConfiguration;
        public TransactionController(IConfiguration config, IEmailConfiguration EmailConfiguration, UserManager<IdentityUser> _userManager, IEmailSender _emailSender, SignInManager<IdentityUser> _signInManager)
        {
            userManager = _userManager;
            emailSender = _emailSender;
            signInManager = _signInManager;
            _emailConfiguration = EmailConfiguration;
            usrFacade = new UserFacade(userManager, emailSender, signInManager);
            url = config.GetSection("API_url").Value;
        }
        private TransactionFacade facade = new TransactionFacade();
        [HttpPost]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll([FromBody]DTParameters param)
        {
            try
            {
                string search = HttpContext.Request.Query["search[value]"].ToString();
                int draw = param.Draw;
                string order = param.Order[0].Column.ToString();
                string orderDir = param.Order[0].Dir.ToString();
                int startRec = param.Start;
                int pageSize = param.Length;
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

        [HttpPost]
        [Route("GetModelWithID")]
        public async Task<IActionResult> GetPost(TransactionRequest req)
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
        [Route("SubmitBuy")]
        public async Task<IActionResult> SubmitBuy([FromBody]TransactionRequest request)
        {
            try
            {
                TransactionResponse response = new TransactionResponse();

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

                response = await facade.SubmitBuy(request.BarangID, request.Harga, username);

                return Ok(response);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("SubmitSell")]
        public async Task<IActionResult> SubmitSell([FromBody]TransactionRequest request)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    TransactionResponse response = new TransactionResponse();

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
   
                    response = await facade.SubmitSell(request.BarangID, request.Harga, username);

                    if (response.IsSuccess)
                    {
                        #region Sent Email to User
                        SendEmail sendEmail = new SendEmail(_emailConfiguration);
                        string contentEmail = configFacade.GetRedaksionalEmail("ContentEmailBuy").Result;
                        string subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailBuy").Result;

                        AuthenticationResponse authResp = await facade.GetUserProfile(username);

                        EmailAddress emailAddress = new EmailAddress();
                        emailAddress.Address = username;
                        emailAddress.Name = authResp.Name;
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

                    return Ok(response);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        //[HttpPost]
        //[Route("DealBuy")]
        //public async Task<IActionResult> DealBuy([FromBody]TransactionRequest request )
        //{
        //    try
        //    {
        //        TransactionResponse response = new TransactionResponse();

        //        string bearer = Request.HttpContext.Request.Headers["Authorization"];
        //        string token = bearer.Substring("Bearer ".Length).Trim();
        //        string username = string.Empty;
        //        if (string.IsNullOrEmpty(token))
        //        {
        //            response.IsSuccess = false;
        //            response.Message = "You don't have access.";
        //            return BadRequest(response);
        //        }

        //        username = sec.ValidateToken(token);
        //        if (username == null)
        //        {
        //            Response.HttpContext.Response.Cookies.Append("access_token", "", new CookieOptions()
        //            {
        //                Expires = DateTime.Now.AddDays(-1)
        //            });
        //            response.IsSuccess = false;
        //            response.Message = "Your session was expired, please re-login.";
        //            return BadRequest(response);
        //        }

        //        response = await facade.SubmitBuy(BarangID, Nominal, username);

        //        return Ok(response);

        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }

        //}

        [HttpPost]
        [Route("DeletePost")]
        public async Task<IActionResult> DeletePost(TransactionRequest req)
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
        public async Task<IActionResult> UpdatePost([FromBody]TransactionRequest req)
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
        [Route("ApproveTransaction")]
        public async Task<IActionResult> ApproveTransaction([FromBody]TransactionRequest req)
        {

            //string bearer = Request.HttpContext.Request.Headers["Authorization"];
            //string token = bearer.Substring("Bearer ".Length).Trim();
            //string username = string.Empty;
            //if (string.IsNullOrEmpty(token))
            //{
            //    resp.IsSuccess = false;
            //    resp.Message = "You don't have access.";
            //    return resp;
            TransactionResponse response = new TransactionResponse();
            try
            {
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
                req.UserName = username;
                response = await facade.ApproveTransaction(req);

                if (response.IsSuccess)
                {
                    #region Sent Email to User
                    TransactionResponse transResp = await facade.GetCurrentTransaction(req);
                    int levelID = transResp.ListTransaction.FirstOrDefault().TrasanctionLevel.ID;
                    SendEmail sendEmail = new SendEmail(_emailConfiguration);
                    string contentEmail = string.Empty;
                    string subjectEmail = string.Empty;

                    AuthenticationResponse authResp = await facade.GetUserProfile(username);

                    EmailAddress emailAddress = new EmailAddress();
                    switch (levelID)
                    {
                        case 1:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailBuy").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailBuy").Result;
                            break;
                        case 2:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailVerifikasi").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailVerifikasi").Result;
                            break;
                        case 3:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailVisit").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailVisit").Result;
                            break;
                        case 4:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailDP").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailDP").Result;
                            break;
                        case 5:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailBBN").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailBBN").Result;
                            break;
                        case 6:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailSTNK").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailSTNK").Result;
                            break;
                        case 7:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailPelunasan").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailPelunasan").Result;
                            break;
                        case 8:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailDelivery").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailDelivery").Result;
                            break;
                    }
                    emailAddress.Address = username;
                    emailAddress.Name = authResp.Name;
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

                return Ok(response);
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

            //    username = sec.ValidateToken(token);
            //    if (username == null)
            //    {
            //        Response.HttpContext.Response.Cookies.Append("access_token", "", new CookieOptions()
            //        {
            //            Expires = DateTime.Now.AddDays(-1)
            //        });
            //        resp.IsSuccess = false;
            //        resp.Message = "Your session was expired, please re-login.";
            //        return resp;
            //    }
            //    req.UserName = username;
            //    resp = await facade.ApproveTransaction(req); 


            //    return resp;
            //}
            //catch (Exception)
            //{
            //    return resp;
            //}
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        var result = await facade.ApproveTransaction(req);

            //        return Ok(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        if (ex.GetType().FullName ==
            //                 "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
            //        {
            //            return NotFound();
            //        }

            //        return BadRequest();
            //    }
            //}

            //return BadRequest();
        }


        [HttpPost]
        [Route("CancelTransaction")]
        public async Task<IActionResult> CancelTransaction([FromBody]TransactionRequest req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await facade.CancelTransaction(req);

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
        [Route("GetJournalByTransaction")]
        public async Task<IActionResult> GetJournalByTransaction([FromBody]TransactionRequest req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await facade.GetJournalByTransaction(req);

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

        //[HttpPost]
        //[Route("SubmitDeal")]
        //public async Task<IActionResult> SubmitDeal([FromBody]TransactionRequest req)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var postId = await facade.SubmitSell(req);

        //            return Ok(postId);

        //        }
        //        catch (Exception)
        //        {
        //            return BadRequest();
        //        }

        //    }

        //    return BadRequest();
        //}

        [HttpPost]
        [Route("GetListBidAndBuy")]
        public async Task<TransactionResponse> ListBidAndBuy([FromBody]TransactionRequest req)
        {
            TransactionResponse resp = new TransactionResponse();
            try
            {
                //string bearer = Request.HttpContext.Request.Headers["Authorization"];
                //string token = bearer.Substring("Bearer ".Length).Trim();
                string token = req.Token;
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
                resp = await facade.GetListBidAndBuy(req);

                return resp;

            }
            catch (Exception ex)
            {
                return resp;
            }


        }

        [HttpPost]
        [Route("GetHistory")]
        public async Task<IActionResult> GetHistory([FromBody]TransactionRequest req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var postId = await facade.GetHistory(req);

                    return Ok(postId);

                }
                catch (Exception)
                {
                    return BadRequest();
                }

            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetAllStatus")]
        public async Task<IActionResult> GetAllHistory()
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var postId = await facade.GetAllStatus();

                    return Ok(postId);

                }
                catch (Exception)
                {
                    return BadRequest();
                }

            }

            return BadRequest();
        }

        [HttpGet]
        [Route("DownloadTransactionByStatus")]
        public IActionResult DownloadTransactionByStatus(string TransactionStatusID, string EndDate)
        {
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            TransactionRequest req = new TransactionRequest();
            req.TransactionStatusID = long.Parse(TransactionStatusID);
            req.EndDate = EndDate;
            var postId = facade.downloadExcel(req);

            //HttpContext.Current.Response.ClearContent();
            //HttpContext.Current.Response.ClearHeaders();
            //HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + "SP3 Document.xlsx");
            //HttpContext.Current.Response.BinaryWrite(pck.GetAsByteArray());
            //HttpContext.Current.Response.End();

            //var file = File(postId, "application/octet-stream");
            //return Ok("adsadsadasd");
            //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            //    result.Content = new ByteArrayContent(postId);
            //    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            //result.Content.Headers.ContentDisposition.FileName = "Report.xslx";
            //    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"); // Text file
            //    result.Content.Headers.ContentLength = postId.Length;

            return File(postId, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report Document.xlsx");
            //return File(postId, "application/octet-stream");
            //return result;

            //return Ok(Path.Combine(url,postId));
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest();
            //}

            //}

            //return BadRequest();
        }

        [HttpPost]
        [Route("GetAllBidByUserProfileID")]
        public async Task<IActionResult> GetAllBidByUserProfileID([FromBody]TransactionRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var postId = await facade.GetAllBidByUserProfileID(request);

                    return Ok(postId);

                }
                catch (Exception)
                {
                    return BadRequest();
                }

            }

            return BadRequest();
        }

        [HttpPost]
        [Route("GetAllAskByUserProfileID")]
        public async Task<IActionResult> GetAlAskByUserProfileID([FromBody]TransactionRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var postId = await facade.GetAllAskByUserProfileID(request);

                    return Ok(postId);

                }
                catch (Exception)
                {
                    return BadRequest();
                }

            }

            return BadRequest();
        }

        [HttpPost]
        [Route("ConfirmWaitingVerification")]
        public async Task<IActionResult> ConfirmWaitingVerification([FromBody]TransactionRequest req)
        {
            TransactionResponse response = new TransactionResponse();
            try
            {
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
                req.UserName = username;
                var responseUserUpdate = usrFacade.UpdateFromWaitingVerification(req.Email, req.Name, req.Telp, req.Alamat, req.Instagram, req.Facebook, req.UserName).Result;
                if (!responseUserUpdate.IsSuccess)
                {
                    response.IsSuccess = false;
                    responseUserUpdate.Message = "Your session was expired, please re-login.";
                    return BadRequest(response);
                }
                response = await facade.ApproveTransaction(req);

                if (response.IsSuccess)
                {
                    #region Sent Email to User
                    TransactionResponse transResp = await facade.GetCurrentTransaction(req);
                    int levelID = transResp.ListTransaction.FirstOrDefault().TrasanctionLevel.ID;
                    SendEmail sendEmail = new SendEmail(_emailConfiguration);
                    string contentEmail = string.Empty;
                    string subjectEmail = string.Empty;

                    AuthenticationResponse authResp = await facade.GetUserProfile(username);

                    EmailAddress emailAddress = new EmailAddress();
                    switch (levelID)
                    {
                        case 1:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailBuy").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailBuy").Result;
                            break;
                        case 2:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailVerifikasi").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailVerifikasi").Result;
                            break;
                        case 3:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailVisit").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailVisit").Result;
                            break;
                        case 4:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailDP").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailDP").Result;
                            break;
                        case 5:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailBBN").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailBBN").Result;
                            break;
                        case 6:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailSTNK").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailSTNK").Result;
                            break;
                        case 7:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailPelunasan").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailPelunasan").Result;
                            break;
                        case 8:
                            contentEmail = configFacade.GetRedaksionalEmail("ContentEmailDelivery").Result;
                            subjectEmail = configFacade.GetRedaksionalEmail("SubjectEmailDelivery").Result;
                            break;
                    }
                    emailAddress.Address = username;
                    emailAddress.Name = authResp.Name;
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

                return Ok(response);
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


    }
}
