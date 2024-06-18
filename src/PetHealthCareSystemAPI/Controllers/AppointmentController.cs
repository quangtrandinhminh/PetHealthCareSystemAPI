using BusinessObject.DTO;
using BusinessObject.DTO.Appointment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetHealthCareSystemAPI.Extensions;
using PetHealthCareSystemAPI.QueryObjects;
using Service.IServices;
using Utility.Constants;

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
            try
            {
                var timeframes = await _appointmentService.GetAllTimeFramesForBookingAsync();

                return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, timeframes));
            }
            catch
            {
                return BadRequest(BaseResponseDto.InternalErrorResponseDto(ResponseMessageConstantsCommon.SERVER_ERROR, null));
            }
        }

        [HttpGet]
        [Route("customer/free-vet-time-frames")]
        public async Task<IActionResult> GetVetFreeTimeFrame([FromQuery] GetFreeVetQueryObject qo)
        {
            try
            {
                if (!DateOnly.TryParse(qo.Date, out DateOnly date))
                {
                    return BadRequest(BaseResponseDto.BadRequestResponseDto(ResponseMessageConstantsCommon.DATE_WRONG_FORMAT, null));
                }

                if (qo.Date == null || qo.TimetableId == null)
                {
                    return BadRequest(BaseResponseDto.BadRequestResponseDto(ResponseMessageConstantsCommon.DATA_NOT_ENOUGH, null));
                }

                var freeVetList = await _appointmentService.GetFreeWithTimeFrameAndDateAsync(date, qo.TimetableId);

                return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, freeVetList));
            }
            catch
            {
                return BadRequest(BaseResponseDto.InternalErrorResponseDto(ResponseMessageConstantsCommon.SERVER_ERROR, null));
            }
        }

        [HttpPost]
        [Route("customer/book")]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentBookRequestDto dto)
        {
            try
            {
                // await _appointmentService.

                return Ok(BaseResponseDto.OkResponseDto("", "No additional data"));
            }
            catch
            {
                return BadRequest(BaseResponseDto.InternalErrorResponseDto(ResponseMessageConstantsCommon.SERVER_ERROR, null));
            }
        }
    }
}
