using ljgb.BusinessLogic;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ljgb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarangController : ControllerBase
    {
        private BarangFacade facade = new BarangFacade();

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
            long Id = request.ID;//Convert.ToInt32(HttpContext.Request.Query["id"]);
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
        public async Task<IActionResult> AddPost([FromBody]BarangRequest model)
        {
            BarangResponse result = new BarangResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ID>0)
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
        public async Task<IActionResult> DeletePost(BarangRequest request)
        {
            BarangResponse response = new BarangResponse();

            
            try
            {
                if (request.ID < 1)
                {
                    return BadRequest();
                }

                response = await facade.DeletePost(request.ID);
              
                return Ok(response);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPost]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody]BarangRequest request)
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
        [Route("GetHargaOTR")]
        public async Task<IActionResult> GetHargaOTR([FromBody]BarangRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await facade.GetHargaOTR(request);

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
        [Route("UploadFile")]
        public async Task<string> Upload(IFormFile file)//, long userId)
        {
            if (file == null || file.Length == 0)
                return "Please select profile picture";

            var folderName = Path.Combine("Resources", "ProfilePics");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var uniqueFileName = "Test.jpg";//$"{userId}_profilepic.png";
            var dbPath = Path.Combine(folderName, uniqueFileName);

            using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return dbPath;
        }
    }
}
