using System;
using System.Threading.Tasks;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace ljgb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarnaController : ControllerBase
    {

        IWarna interfaceWarna;
        public WarnaController(IWarna _interfaceWarna)
        {
            interfaceWarna = _interfaceWarna;
        }

        [HttpGet]
        [Route("GetWarna")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await interfaceWarna.GetWarna();
                if (categories == null)
                {
                    return NotFound();
                }

                return Ok(categories);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("GetPosts")]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                var posts = await interfaceWarna.GetPosts();
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
                var post = await interfaceWarna.GetPost(postId);

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
                    var postId = await interfaceWarna.AddPost(model);
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
                result = await interfaceWarna.DeletePost(postId);
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
                    await interfaceWarna.UpdatePost(model);

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