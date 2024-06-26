﻿using BusinessObject.DTO;
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
        [Route("customer/time-frames")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTimeFrameForBooking()
        {

            var timeframes = await _appointmentService.GetAllTimeFramesForBookingAsync();

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, timeframes));

        }

        [HttpGet]
        [Route("customer/free-vet-time-frames")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVetFreeTimeFrame([FromQuery] AppointmentDateTimeQueryDto qo)
        {
            var freeVetList = await _appointmentService.GetFreeWithTimeFrameAndDateAsync(qo);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, freeVetList));
        }

        [HttpPost]
        [Route("customer/book")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentBookRequestDto dto)
        {
            var ownerId = User.GetUserId();

            var response = await _appointmentService.BookOnlineAppointmentAsync(dto, ownerId);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, response));
        }

        [HttpGet]
        [Route("staff/appointments")]
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
    }
}
