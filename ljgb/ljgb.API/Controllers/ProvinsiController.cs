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
    public class ProvinsiController :ControllerBase
    {
        private ProvinsiFacade facade = new ProvinsiFacade();
        [HttpPost]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll(ProvinsiRequest req)
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
        public async Task<IActionResult> GetPost(ProvinsiRequest req)
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
        public async Task<IActionResult> AddPost([FromBody]ProvinsiRequest req)
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
        public async Task<IActionResult> DeletePost(ProvinsiRequest req)
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
        public async Task<IActionResult> UpdatePost([FromBody]ProvinsiRequest req)
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
