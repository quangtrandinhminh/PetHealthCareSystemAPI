using BusinessObject.DTO;
using BusinessObject.DTO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Service.Services;
using Utility.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetHealthCareSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private IService _iservice;

        public ServiceController (IService service)
        {
            _iservice = service;
        }

        // GET: api/<ServiceController>
        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Route("GetAllService")]
        public async Task<IActionResult> Get()
        {
            var list = await _iservice.GetAllService();

            return Ok(BaseResponseDto.OkResponseDto(list, "No additional data"));
        }

        // GET api/<ServiceController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ServiceController>
        [HttpPost]
        [Route("Create Service")]
        public async Task<OkObjectResult> PostAsync([FromBody] ServiceResponseDto dto)
        {
            await _iservice.CreateServiceAsync(dto);

            return Ok(BaseResponseDto.OkResponseDto(ReponseMessageConstantsPet.ADD_PET_SUCCESS));
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
