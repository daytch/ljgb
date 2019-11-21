using ljgb.BusinessLogic;
using ljgb.Common.Requests;
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
        private readonly IConfiguration _config;
        private string url = "";
        public TransactionController(IConfiguration config)
        {
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
        public async Task<IActionResult> SubmitBuy([FromBody]int BarangID, int Nominal)
        {
            try
            {
                var postId = await facade.SubmitBuy(BarangID, Nominal);

                return Ok(postId);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("SubmitSell")]
        public async Task<IActionResult> SubmitSell([FromBody]int BarangID, int Nominal)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var postId = await facade.SubmitSell(BarangID, Nominal);

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
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await facade.ApproveTransaction(req);

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

        [HttpPost]
        [Route("SubmitDeal")]
        public async Task<IActionResult> SubmitDeal([FromBody]TransactionRequest req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var postId = await facade.SubmitSell(req);

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
    }
}
