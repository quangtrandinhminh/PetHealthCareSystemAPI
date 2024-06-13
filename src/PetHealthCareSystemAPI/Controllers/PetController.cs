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
        [Route("GetAllPetsForCustomer")]
        public async Task<IActionResult> GetAllPetsForCustomer()
        {
            var id = User.GetUserId();

            var list = await _petService.GetAllPetsForCustomerAsync(id);

            return Ok(BaseResponseDto.OkResponseDto(list, "No additional data"));
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Route("GetPetForCustomer/{id:int}")]
        public async Task<IActionResult> GetPetForCustomer([FromRoute] int id)
        {
            var petId = User.GetUserId();

            var list = await _petService.GetAllPetsForCustomerAsync(petId);

            foreach (var pet in list)
            {
                if (pet.Id == id) return Ok(BaseResponseDto.OkResponseDto(pet, "No additional data"));
            }

            return Ok(BaseResponseDto.NotFoundResponseDto("Không tìm thấy thú cưng của bạn"));
        }

        // POST api/<PetController>
        [HttpPost]
        [Authorize(Roles = "Customer")]
        [Route("AddPet")]
        public async Task<IActionResult> AddPet([FromBody] PetRequestDto dto)
        {
            var id = User.GetUserId();

            await _petService.CreatePetAsync(dto, id);

            return Ok(BaseResponseDto.OkResponseDto(ReponseMessageConstantsPet.ADD_PET_SUCCESS));
        }

        // PUT api/<PetController>/5
        [HttpPut]
        [Route("UpdatePet")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdatePet([FromBody] PetUpdateRequestDto dto)
        {
            await _petService.UpdatePetAsync(dto);

            return Ok(BaseResponseDto.OkResponseDto(ReponseMessageConstantsPet.UPDATE_PET_SUCCESS));
        }

        // DELETE api/<PetController>/5
        [HttpDelete]
        [Authorize(Roles = "Customer")]
        [Route("RemovePet/{id:int}")]
        public async Task<IActionResult> RemovePet(int id)
        {
            await _petService.DeletePetAsync(id);

            return Ok(BaseResponseDto.OkResponseDto(ReponseMessageConstantsPet.DELETE_PET_SUCCESS));
        }
    }
}
