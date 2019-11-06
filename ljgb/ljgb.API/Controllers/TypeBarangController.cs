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
    public class TypeBarangController : ControllerBase
    {
        private TypeBarangFacade facade = new TypeBarangFacade();
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

        [HttpPost]
        [Route("GetAllWithModelBarangID")]
        public async Task<IActionResult> GetAllWithModelID([FromBody]TypeBarangRequest request)
        {
            try
            {
                var models = await facade.GetAllWithModelID(request);
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
        public async Task<IActionResult> GetPost([FromBody]TypeBarangRequest request)
        {
           

            try
            {
                var post = await facade.GetPost(request);

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
        public async Task<IActionResult> AddPost([FromBody]TypeBarangRequest model)
        {
            TypeBarangResponse response = new TypeBarangResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ID >0)
                    {
                        response = await facade.UpdatePost(model);
                    }
                    else
                    {
                        response = await facade.AddPost(model);
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

        [HttpPost]
        [Route("DeletePost")]
        public async Task<IActionResult> DeletePost([FromBody]TypeBarangRequest request)
        {

            try
            {
                var result = await facade.DeletePost(request);
               
                return Ok(result);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


        [HttpPost]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody]TypeBarangRequest request)
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

        [HttpPost]
        [Route("GetTypeByKotaIDMerkIDModelID")]
        public async Task<IActionResult> GetTypeByKotaIDMerkIDModelID([FromBody]TypeBarangRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await facade.GetTypeByKotaIDMerkIDModelID(request);

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
