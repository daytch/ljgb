using ljgb.BusinessLogic;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ljgb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KotaController : ControllerBase
    {
        private KotaFacade facade = new KotaFacade();
        //[HttpPost]
        //[Route("GetAll")]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        var models = await facade.GetAll();
        //        if (models == null)
        //        {
        //            return NotFound();
        //        }

        //        return Ok(models);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}


        [HttpGet]
        [Route("GetAllForDropdown")]
        public async Task<IActionResult> GetAllForDropdown(int ProvinsiID)
        {
            KotaResponse response = new KotaResponse();
            try
            {
                response.ListKotas = await facade.GetAllForDropdown(ProvinsiID);
                response.IsSuccess = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.ToString();
                return BadRequest(ex);
            }
            return Ok(response);
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



        [HttpPost]
        [Route("GetModelWithID")]
        public async Task<IActionResult> GetPost(KotaRequest req)
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
        public async Task<IActionResult> AddPost([FromBody]KotaRequest req)
        {
            KotaResponse response = new KotaResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.ID> 0)
                    {
                        response = await facade.UpdatePost(req);
                    }
                    else
                    {
                        response = await facade.AddPost(req);
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
        public async Task<IActionResult> DeletePost(KotaRequest req)
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
        public async Task<IActionResult> UpdatePost([FromBody]KotaRequest req)
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


        [HttpGet]
        [Route("GetAllKotaWithoutFilter")]
        public async Task<IActionResult> GetAllKotaWithoutFilter()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await facade.GetAllWithoutFilter();

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
