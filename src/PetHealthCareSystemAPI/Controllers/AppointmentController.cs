using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetHealthCareSystemAPI.Extensions;
using PetHealthCareSystemAPI.QueryObjects;
using Service.IServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetHealthCareSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController(IServiceProvider serviceProvider) : ControllerBase
    {
        private readonly IAppointmentService _appointmentService =
            serviceProvider.GetRequiredService<IAppointmentService>();

        [HttpGet]
        [Route("customer/time-frames")]
        public async Task<IActionResult> GetTimeFrameForBooking()
        {
            var timeframes = await _appointmentService.GetAllTimeFramesForBookingAsync();

            return Ok(BaseResponseDto.OkResponseDto(timeframes));
        }

        [HttpGet]
        [Route("customer/free-vet-time-frames")]
        public async Task<IActionResult> GetVetFreeTimeFrame([FromQuery] GetFreeVetQueryObject qo)
        {
            if (!DateOnly.TryParse(qo.Date, out DateOnly date))
            {
                return BadRequest(BaseResponseDto.BadRequestResponseDto(null));
            }

            if (qo.Date == null || qo.TimetableId == null)
            {
                return BadRequest(BaseResponseDto.BadRequestResponseDto(null));
            }

            var freeVetList = await _appointmentService.GetFreeWithTimeFrameAndDateAsync(date, qo.TimetableId);

            return Ok(BaseResponseDto.OkResponseDto(freeVetList));
        }

        [HttpPost]
        [Route("customer/book")]
        public async Task<IActionResult> BookAppointment()
        {
            var ownerId = User.GetUserId();

            return Ok(BaseResponseDto.OkResponseDto("", "No additional data"));
        }
    }
}
