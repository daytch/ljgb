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
    public class NegoBarangController : ControllerBase
    {
        private NegoBarangFacade facade = new NegoBarangFacade();
        [HttpPost]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var models = await facade.GetAll();
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
        public async Task<IActionResult> GetPost(NegoBarangRequest req)
        {
            if (req ==null)
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
        [Route("SubmitBid")]
        public async Task<IActionResult> SubmitBid([FromBody]NegoBarangRequest request)
        {
            try
            {
                var result = await facade.SubmitBid(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok();
        }


        [HttpPost]
        [Route("SubmitAsk")]
        public async Task<IActionResult> SubmitAsk([FromBody]NegoBarangRequest request)
        {
            try
            {
                var result = await facade.Submitask(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok();
        }


        [HttpPost]
        [Route("DeletePost")]
        public async Task<IActionResult> DeletePost(NegoBarangRequest req)
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
        public async Task<IActionResult> UpdatePost([FromBody]NegoBarangRequest req)
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
    }
}
