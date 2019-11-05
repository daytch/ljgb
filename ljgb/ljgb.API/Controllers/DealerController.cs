using System;
using System.Threading.Tasks;
using ljgb.API.Core;
using ljgb.BusinessLogic;
using ljgb.Common.Responses;
using ljgb.DataAccess.Model;
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
    }
}