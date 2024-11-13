using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHealthCareSystemAPI.Extensions;
using Repository.Models;
using Repository.Models.Service;
using Service.IServices;
using Utility.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetHealthCareSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceController : ControllerBase
    {
        private IService _iservice;

        public ServiceController (IService service)
        {
            _iservice = service;
        }

        // GET: api/<ServiceController>
        [HttpGet]
        [AllowAnonymous]
        [Route("get-all")]
        public async Task<IActionResult> Get()
        {
            var list = await _iservice.GetAllServiceAsync();

            return Ok(BaseResponseDto.OkResponseDto(list, "No additional data"));
        }

        // GET api/<ServiceController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var list = await _iservice.GetServiceBydId(id);

            return Ok(BaseResponseDto.OkResponseDto(list, "No additional data"));
        }

        // POST api/<ServiceController>
        [HttpPost]
        [Route("add")]
        public async Task<OkObjectResult> PostAsync([FromBody] ServiceRequestDto dto)
        {
            var userId = User.GetUserId();
            await _iservice.CreateServiceAsync(dto, userId);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsPet.ADD_PET_SUCCESS));
        }

        // PUT api/<ServiceController>/5
        [HttpPut]
        [Route("update")]
        public async Task<OkObjectResult> Put([FromBody] ServiceUpdateDto serviceRequestDto)
        {
            var userId = User.GetUserId();
            await _iservice.UpdateServiceAsync(serviceRequestDto, userId);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsPet.UPDATE_PET_SUCCESS));
        }

        // DELETE api/<ServiceController>/5
        [HttpDelete("{id}")]
        public async Task<OkObjectResult> Delete(int id, int deleteBy)
        {
            await _iservice.DeleteServiceAsync(id, deleteBy);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsPet.DELETE_PET_SUCCESS));
        }
    }
}
