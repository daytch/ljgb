using ljgb.BusinessLogic;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ljgb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarangController : ControllerBase
    {
        private BarangFacade facade = new BarangFacade();

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
        
        [HttpGet]
        [Route("GetAllForHomePage")]
        public IActionResult GetAllForHomePage([FromQuery]string city)
        {
            BarangResponse respon = new BarangResponse();
            try
            {
                respon = facade.GetAllForHomePage(city);
                respon.IsSuccess = true;
                respon.Message = "Success";

                return Ok(respon);
            }
            catch (Exception ex)
            {
                respon.Message = ex.Message;
                respon.IsSuccess = false;
                return BadRequest(respon);
            }
        }

        [HttpGet]
        [Route("GetBarangDetail")]
        public async Task<IActionResult> GetBarangDetail(int Id)
        {
            if (Id < 1)
            {
                return BadRequest();
            }

            try
            {
                BarangResponse post = await facade.GetBarangDetail(Id);
                post.IsSuccess = true;
                post.Message = "Success";

                if (post == null)
                {
                    return NotFound();
                }

                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetBidPosition")]
        public async Task<IActionResult> GetBidPosition(int Id,int Nominal)
        {
            if (Id < 1)
            {
                return BadRequest();
            }

            try
            {
                Position post = await facade.GetBidPosition(Id,Nominal);

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

        [HttpGet]
        [Route("GetAskPosition")]
        public async Task<IActionResult> GetAskPosition(int Id, int Nominal)
        {
            if (Id < 1)
            {
                return BadRequest();
            }

            try
            {
                Position post = await facade.GetAskPosition(Id, Nominal);

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

        [HttpGet]
        [Route("GetAllAsksById")]
        public IActionResult GetAllAsksById([FromQuery]BarangRequest request)
        {
            int Id = request.ID;//Convert.ToInt32(HttpContext.Request.Query["id"]);
            int start = Convert.ToInt32(HttpContext.Request.Query["start"]);
            int limit = Convert.ToInt32(HttpContext.Request.Query["limit"]);
            int max = Convert.ToInt32(HttpContext.Request.Query["max"]);

            if (Id < 1)
            {
                return BadRequest();
            }

            try
            {
                BarangResponse post = facade.GetAllAsksById(request);
                post.IsSuccess = true;
                post.Message = "Success";
                if (post == null)
                {
                    return NotFound();
                }

                return Ok(post);
            }
            catch (Exception ex)
            {                
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("GetModelWithID")]
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
        public async Task<IActionResult> AddPost([FromBody]Barang model)
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


        [HttpPost]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody]Barang model)
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
    }
}
