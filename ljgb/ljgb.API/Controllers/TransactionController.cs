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
        public async Task<IActionResult> GetAll()
        {
            

            try
            {
                string search = HttpContext.Request.Query["search[value]"].ToString();
                string order = HttpContext.Request.Query["order[0][column]"];
                string orderDir = HttpContext.Request.Query["order[0][dir]"];
                int startRec = Convert.ToInt32(HttpContext.Request.Query["start"]);
                int pageSize = Convert.ToInt32(HttpContext.Request.Query["length"]);
                int draw = Convert.ToInt32(HttpContext.Request.Query["draw"]);

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
        [Route("AddPost")]
        public async Task<IActionResult> AddPost([FromBody]TransactionRequest req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var postId = await facade.AddPost(req);

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
    }
}
