using BusinessObject.DTO;
using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Hospitalization;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetHealthCareSystemAPI.Extensions;
using Service.IServices;
using Service.Services;
using System.Collections.Generic;
using Utility.Constants;

namespace PetHealthCareSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HospitalizationController(IHospitalizationService hospitalizationService) : ControllerBase
    {
        [HttpGet]
        [Route("time-frames")]
        public async Task<IActionResult> GetTimeFrameForBooking()
        {

            var timeframes = await hospitalizationService.GetAllTimeFramesForHospitalizationAsync();

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, timeframes));

        }

        [HttpGet]
        [Route("free-vet-time-frames")]
        public async Task<IActionResult> GetVetFreeTimeFrame([FromQuery] DateTimeQueryDto qo)
        {
            var freeVetList = await hospitalizationService.GetFreeWithTimeFrameAndDateAsync(qo);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, freeVetList));
        }

        [HttpGet]
        [Route("cage/available")]
        public async Task<IActionResult> GetAvailableCageByDate([FromQuery] DateTimeQueryDto qo)
        {
            var cages = await hospitalizationService.GetAvailableCageByDate(qo);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, cages));
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllHospitalization(int pageNumber, int pageSize)
        {
            var hospitalization = await hospitalizationService.GetAllHospitalization(pageNumber, pageSize);
            return Ok(BaseResponseDto.OkResponseDto(hospitalization));
        }

        [HttpGet("{hospitalizationId}")]
        public async Task<IActionResult> GetHospitalizationById(int hospitalizationId)
        {
            var hospitalization = await hospitalizationService.GetHospitalizationById(hospitalizationId);
            return Ok(BaseResponseDto.OkResponseDto(hospitalization));
        }

        [HttpGet("medical-record/{medicalRecordId}")]
        public async Task<IActionResult> GetAllHospitalizationByMedicalRecordId(int medicalRecordId, int pageNumber,
            int pageSize)
        {
            var hospitalization = await hospitalizationService.GetAllHospitalizationByMedicalRecordId(medicalRecordId, pageNumber, pageSize);
            return Ok(BaseResponseDto.OkResponseDto(hospitalization));
        }

        /*[HttpGet("status")]
        public IActionResult GetHospitalizationStatus()
        {
            var hospitalizationStatus = hospitalizationService.GetHospitalizationStatus();
            return Ok(BaseResponseDto.OkResponseDto(hospitalizationStatus));
        }*/

        [HttpPost]
        [Route("create")]
        [Authorize(Roles="Staff")]
        public async Task<IActionResult> CreateHospitalization( [FromBody] HospitalizationRequestDto dto)
        {
            var staffId = User.GetUserId();
            await hospitalizationService.CreateHospitalization(dto, staffId);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsHospitalization.ADD_HOSPITALIZATION_SUCCESS));
        }

        [HttpPut]
        [Authorize(Roles = "Vet")]
        public async Task<IActionResult> UpdateHospitalization( /*[FromBody] HospitalizationRequestDto dto*/)
        {
            //var vetId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            //await _hospitalizationService.UpdateHospitalization(dto, vetId);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteHospitalization(int id)
        {
            //var deleteBy = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            //await _hospitalizationService.DeleteHospitalization(id, deleteBy);
            return Ok();
        }
    }
}
