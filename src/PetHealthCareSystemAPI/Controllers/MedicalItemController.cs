using BusinessObject.DTO.Service;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Utility.Constants;
using BusinessObject.DTO.MedicalItem;

namespace PetHealthCareSystemAPI.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalItemController : Controller
    {
        private IMedicalService _medicalService;

        public MedicalItemController(IMedicalService medicalService)
        {
            _medicalService = medicalService;
        }

        // GET: api/<MedicalItemController>
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> Get()
        {
            var list = await _medicalService.GetAllMedicalItem();

            return Ok(BaseResponseDto.OkResponseDto(list, "No additional data"));
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        [Route("create")]
        public async Task<OkObjectResult> PostAsync([FromBody] MedicalResponseDto dto)
        {
            await _medicalService.CreateMedicalItem(dto);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsPet.ADD_PET_SUCCESS));
        }

        // PUT api/<ServiceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ServiceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
