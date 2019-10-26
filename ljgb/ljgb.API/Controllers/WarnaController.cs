using System;
using System.Threading.Tasks;
using ljgb.BusinessLogic;
using ljgb.DataAccess.Model;
using Microsoft.AspNetCore.Mvc;

namespace ljgb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarnaController : ControllerBase
    {
        private WarnaFacade facade = new WarnaFacade();
        [HttpGet]
        [Route("GetWarna")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await facade.GetCategories();
                if (categories == null)
                {
                    return NotFound();
                }

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetPosts")]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                var posts = await facade.GetPosts();
                if (posts == null)
                {
                    return NotFound();
                }

                return Ok(posts);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetPost")]
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
        public async Task<IActionResult> AddPost([FromBody]Warna model)
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

        [HttpDelete]
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


        [HttpPut]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody]Warna model)
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