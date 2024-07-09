using Azure.Core;
using BusinessObject.DTO;
using BusinessObject.DTO.Vet;
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
    public class VetController : ControllerBase
    {
        private readonly IUserService _userService;

        public VetController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetAllUsersByRoleAsync(3));
        }

        // GET api/<VetController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<VetController>
        [HttpPost]
        [Route("CreateVetAsync")]
        public async Task<IActionResult> PostAsync([FromBody] VetRequestDto dto)
        {
            await _userService.CreateVetAsync(dto);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageIdentitySuccess.REGIST_USER_SUCCESS));
        }

        // PUT api/<VetController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VetController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
