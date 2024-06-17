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
            var ownerId = User.GetUserId();

            var list = await _petService.GetAllPetsForCustomerAsync(ownerId);

            return Ok(BaseResponseDto.OkResponseDto(list));
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Route("GetPetForCustomer/{id:int}")]
        public async Task<IActionResult> GetPetForCustomer([FromRoute] int id)
        {
            var ownerId = User.GetUserId();

            var pet = _petService.GetPetForCustomerAsync(ownerId, id);

            if (pet != null)
            {
                return Ok(BaseResponseDto.OkResponseDto(pet));
            }

            return Ok(BaseResponseDto.NotFoundResponseDto(ReponseMessageConstantsPet.PET_NOT_FOUND));
        }

        // POST api/<PetController>
        [HttpPost]
        [Authorize(Roles = "Customer")]
        [Route("AddPet")]
        public async Task<IActionResult> AddPet([FromBody] PetRequestDto dto)
        {
            var ownerId = User.GetUserId();

            await _petService.CreatePetAsync(dto, ownerId);

            return Ok(BaseResponseDto.OkResponseDto(ReponseMessageConstantsPet.ADD_PET_SUCCESS));
        }

        // PUT api/<PetController>/5
        [HttpPut]
        [Route("UpdatePet")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdatePet([FromBody] PetUpdateRequestDto dto)
        {
            var ownerId = User.GetUserId();

            await _petService.UpdatePetAsync(dto, ownerId);

            return Ok(BaseResponseDto.OkResponseDto(ReponseMessageConstantsPet.UPDATE_PET_SUCCESS));
        }

        // DELETE api/<PetController>/5
        [HttpDelete]
        [Authorize(Roles = "Customer")]
        [Route("RemovePet/{id:int}")]
        public async Task<IActionResult> RemovePet(int id)
        {
            var ownerId = User.GetUserId();

            await _petService.DeletePetAsync(id, ownerId);

            return Ok(BaseResponseDto.OkResponseDto(ReponseMessageConstantsPet.DELETE_PET_SUCCESS));
        }
    }
}
