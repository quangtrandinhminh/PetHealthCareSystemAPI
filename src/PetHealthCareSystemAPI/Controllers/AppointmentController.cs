using BusinessObject.DTO;
using BusinessObject.DTO.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetHealthCareSystemAPI.Extensions;
using Service.IServices;
using Utility.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetHealthCareSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController(IServiceProvider serviceProvider) : ControllerBase
    {
        private readonly IAppointmentService _appointmentService =
            serviceProvider.GetRequiredService<IAppointmentService>();

        [HttpGet]
        [Route("time-frames")]
        public async Task<IActionResult> GetTimeFrameForBooking()
        {

            var timeframes = await _appointmentService.GetAllTimeFramesForBookingAsync();

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, timeframes));

        }

        [HttpGet]
        [Route("free-vet-time-frames")]
        public async Task<IActionResult> GetVetFreeTimeFrame([FromQuery] DateTimeQueryDto qo)
        {
            var freeVetList = await _appointmentService.GetFreeWithTimeFrameAndDateAsync(qo);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, freeVetList));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetAppointmentById([FromRoute] int id)
        {
            var response = await _appointmentService.GetAppointmentByAppointmentId(id);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, response));
        }

        [HttpPost]
        [Route("book")]
        [Authorize(Roles = "Customer, Staff")]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentBookRequestDto dto)
        {
            var ownerId = User.GetUserId();

            var response = await _appointmentService.BookAppointmentAsync(dto, ownerId);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, response));
        }

        [HttpGet]
        [Route("appointments")]
        public async Task<IActionResult> GetAllAppointment([FromQuery] int pageNumber = 1, int pageSize = 10)
        {
            var response = await _appointmentService.GetAllAppointmentsAsync(pageNumber, pageSize);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, response));
        }

        [HttpGet]
        [Route("vet/appointments/{id:int}")]
        public async Task<IActionResult> GetAllAppointmentForVet([FromRoute] int id, [FromQuery] string? date, int pageNumber = 1, int pageSize = 10)
        {
            var response = await _appointmentService.GetVetAppointmentsAsync(id, date, pageNumber, pageSize);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, response));
        }

        [HttpGet]
        [Route("customer/appointments")]
        public async Task<IActionResult> GetAllAppointmentForCustomer([FromQuery] string? date, int pageNumber = 1, int pageSize = 10)
        {
            var userId = User.GetUserId();

            var response = await _appointmentService.GetUserAppointmentsAsync(pageNumber, pageSize, userId, date);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, response));
        }

        [HttpGet]
        [Route("filter")]
        public async Task<IActionResult> GetAppointmentWithFilter([FromQuery] AppointmentFilterDto filter,
            int pageNumber = 1, int pageSize = 10)
        {
            var response = await _appointmentService.GetAppointmentWithFilter(filter, pageNumber, pageSize);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, response));
        }

        [HttpPut]
        [Route("{appointmentId:int}")]
        [Authorize(Roles = "Vet, Staff")]
        public async Task<IActionResult> UpdateAppointment([FromRoute] int appointmentId, [FromBody] AppointmentBookRequestDto dto)
        {
            var updatedById = User.GetUserId();

            var response = await _appointmentService.UpdateStatusToDone(appointmentId, updatedById);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, response));
        }

    }
}
