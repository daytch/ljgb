using System;
using System.Threading.Tasks;
using ljgb.BusinessLogic;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ljgb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        private DealerFacade facade = new DealerFacade();
        
        [HttpGet]
        [Route("GetAllForDropdown")]
        public async Task<IActionResult> GetAllForDropdown(int KotaID)
        {
            DealerResponse response = new DealerResponse();
            try
            {
                response.ListDealer = await facade.GetAllForDropdown(KotaID);
                response.IsSuccess = true;
                response.Message = "Success";

                if (response == null)
                {
                    return NotFound();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                string search = HttpContext.Request.Query["search[value]"].ToString();
                int draw = Convert.ToInt32(HttpContext.Request.Query["draw"]);
                string order = HttpContext.Request.Query["order[0][column]"];
                string orderDir = HttpContext.Request.Query["order[0][dir]"];
                int startRec = Convert.ToInt32(HttpContext.Request.Query["start"]);
                int pageSize = Convert.ToInt32(HttpContext.Request.Query["length"]);
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

        //[HttpPost]
        //[Route("GetModelWithID")]
        //public async Task<IActionResult> GetPost(long postId)
        //{
        //    if (postId < 1)
        //    {
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        var post = await facade.GetPost(postId);

        //        if (post == null)
        //        {
        //            return NotFound();
        //        }

        //        return Ok(post);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}

        [HttpPost]
        [Route("AddPost")]
        public async Task<IActionResult> AddPost([FromBody]DealerRequest model)
        {
            DealerResponse result = new DealerResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ID > 0)
                    {
                        result = await facade.UpdatePost(model);
                    }
                    else
                    {
                        result = await facade.AddPost(model);
                    }
                    return Ok(result);
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
        public async Task<IActionResult> DeletePost(long ID)
        {
            DealerResponse response = new DealerResponse();


            try
            {
                if (ID < 1)
                {
                    return BadRequest();
                }

                response = await facade.DeletePost(ID);

                return Ok(response);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


        [HttpPost]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody]DealerRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await facade.UpdatePost(request);

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