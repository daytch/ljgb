using ljgb.BusinessLogic;
using ljgb.Common.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ljgb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private TransactionFacade facade = new TransactionFacade();
        [HttpPost]
        [Route("GetAll")]
<<<<<<< HEAD
        public async Task<IActionResult> GetAll([FromBody]DTParameters param)
        {
            
=======
        public async Task<IActionResult> GetAll()
        {           
>>>>>>> a23181c11cd32eaf2d43d6d944d5c461df991b6f

            try
            {

                
                //var test1 = HttpContext.Request.Form;
             
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
    }
}
