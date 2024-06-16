using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using PetHealthCareSystemAPI.Extensions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetHealthCareSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        [HttpGet]
        [Route("GetTimeFrame")]
        public async Task<IActionResult> GetTimeFrame()
        {
            var ownerId = User.GetUserId();

            return Ok(BaseResponseDto.OkResponseDto("", "No additional data"));
        }
    }
}
