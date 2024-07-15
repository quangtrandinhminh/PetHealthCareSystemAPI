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
        public async Task<IActionResult> GetTimeFrameForHospitalization()
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
        public async Task<IActionResult> GetAvailableCageByDate()
        {
            var cages = await hospitalizationService.GetAvailableCageByDate();

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
        public async Task<IActionResult> UpdateHospitalization([FromBody] HospitalizationRequestDto dto, int vetId)
        {
            await hospitalizationService.UpdateHospitalization(dto, vetId);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsHospitalization.UPDATE_HOSPITALIZATION_SUCCESS));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteHospitalization(int id, int deleteBy)
        {
            await hospitalizationService.DeleteHospitalization(id, deleteBy);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsHospitalization.DELETE_HOSPITALIZATION_SUCCESS));
        }

        [HttpPut]
        [Route("HospitalDischarge")]
        [Authorize(Roles = "Vet")]
        public async Task<IActionResult> HospitalDischarge(int medicalRecordId, int VetId)
        {
            await hospitalizationService.HospitalDischarge(medicalRecordId, VetId);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsHospitalization.UPDATE_HOSPITALIZATION_SUCCESS));
        }

        [HttpGet]
        [Route("dropdown-data")]
        public IActionResult GetHospitalizaionDropdownData()
        {
            var response = hospitalizationService.GetHospitalizaionDropdownData();
            return Ok(BaseResponseDto.OkResponseDto(response));
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [Route("GetAllPetInHospitalization")]
        public IActionResult GetAllPetInHospitalization()
        {
            var response = hospitalizationService.GetAllPetInHospitalization();
            return Ok(BaseResponseDto.OkResponseDto(response));
        }

        [HttpGet]
        [Authorize(Roles = "Vet")]
        [Route("CheckHospitalizaionByVetId")]
        public async Task<IActionResult> CheckHospitalizaionByVetId(int id)
        {
            var response = await hospitalizationService.CheckHospitalizaionByVetId(id);
            return Ok(BaseResponseDto.OkResponseDto(response));
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [Route("CheckCreateHospitalization")]
        public async Task<IActionResult> CheckCreateHospitalization(int medicalRecordId)
        {
            var response = await hospitalizationService.CheckCreateHospitalization(medicalRecordId);
            return Ok(BaseResponseDto.OkResponseDto(response));
        }
    }
}
