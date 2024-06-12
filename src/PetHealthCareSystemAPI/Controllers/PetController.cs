using BusinessObject.DTO;
using BusinessObject.DTO.Pet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHealthCareSystemAPI.Extensions;
using Service.IServices;
using Utility.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetHealthCareSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private readonly IPetService _petService;

        public PetController(IPetService petService)
        {
            _petService = petService;
        }

        // GET: api/<PetController>
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var id = User.GetUserId();

            var list = await _petService.GetAllPetsForCustomerAsync(id);

            return Ok(BaseResponseDto.OkResponseDto(list, "No additional data"));
        }

        // GET api/<PetController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PetController>
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Post([FromBody] PetRequestDto dto)
        {
            var id = User.GetUserId();

            dto.OwnerID = id;
            await _petService.CreatePetAsync(dto);

            return Ok(BaseResponseDto.OkResponseDto(ReponseMessageConstantsPet.ADD_PET_SUCCESS));
        }

        // PUT api/<PetController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PetController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
