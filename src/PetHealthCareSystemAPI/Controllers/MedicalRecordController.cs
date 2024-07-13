using System.Security.Claims;
using BusinessObject.DTO;
using BusinessObject.DTO.MedicalRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHealthCareSystemAPI.Extensions;
using Service.IServices;
using Utility.Constants;
using Utility.Exceptions;

namespace PetHealthCareSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicalRecordController : Controller
    {
        private readonly IMedicalService _medicalService;

        public MedicalRecordController(IMedicalService medicalService)
        {
            _medicalService = medicalService;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, int pageSize = 10)
        {
            var list = await _medicalService.GetAllMedicalRecord(pageNumber, pageSize);
            return Ok(BaseResponseDto.OkResponseDto(list));
        }

        [HttpGet]
        [Route("{medicalRecordId:int}")]
        public async Task<IActionResult> GetById(int medicalRecordId)
        {
            var medicalRecord = await _medicalService.GetMedicalRecordById(medicalRecordId);
            return Ok(BaseResponseDto.OkResponseDto(medicalRecord));
        }

        [HttpGet]
        [Route("pet/{petId:int}")]
        public async Task<IActionResult> GetByPetId([FromQuery] int petId, int pageNumber = 1, int pageSize = 10)
        {
            var medicalRecord = await _medicalService.
                GetAllMedicalRecordByPetId(petId, pageNumber, pageSize);
            return Ok(BaseResponseDto.OkResponseDto(medicalRecord));
        }

        [HttpGet]
        [Route("hospitalization")]
        public async Task<IActionResult> GetHospitalization([FromQuery] int pageNumber = 1, int pageSize = 10)
        {
            var medicalRecord = await _medicalService.GetAllMedicalRecordForHospitalization(pageNumber, pageSize);
            return Ok(BaseResponseDto.OkResponseDto(medicalRecord));
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Vet")]
        public async Task<IActionResult> Create([FromBody] MedicalRecordRequestDto medicalRecordDto)
        {
            var vetId = User.GetUserId();
            return Ok(BaseResponseDto.OkResponseDto(await _medicalService.CreateMedicalRecord(medicalRecordDto, vetId)));
        }

        [HttpGet]
        [Route("pet/{petId:int}/appointment/{appointmentId:int}")]
        public async Task<IActionResult> GetByPetIdAndAppointmentId([FromRoute] int petId, int appointmentId, [FromQuery] int pageNumber = 1, int pageSize = 10)
        {
            var medicalRecord = await _medicalService.
                GetMedicalRecordByPetIdAndAppointmentId(petId, appointmentId);
            return Ok(BaseResponseDto.OkResponseDto(medicalRecord));
        }

        /*[HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] MedicalRecordRequestDto medicalRecordDto)
        {
                var medicalRecord = await _medicalService.UpdateMedicalRecord(medicalRecordDto);
                return Ok(BaseResponseDto.OkResponseDto((ResponseMessageConstantsTransaction.ADD_TRANSACTION_SUCCESS)));
        }*/
    }
}
