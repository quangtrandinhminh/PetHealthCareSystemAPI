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

        [Authorize(Roles = "Customer")]
        [HttpGet]
        [Route("customer/all")]
        public async Task<IActionResult> GetAllPetsForCustomer()
        {
            var ownerId = User.GetUserId();

            var list = await _petService.GetAllPetsForCustomerAsync(ownerId);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, list));
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Route("customer/{id:int}")]
        public async Task<IActionResult> GetPetForCustomer([FromRoute] int id)
        {
            var ownerId = User.GetUserId();

            var pet = await _petService.GetPetForCustomerAsync(ownerId, id);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, pet));
        }

        [HttpGet]
        [Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> GetPetById([FromRoute] int id)
        {
            var pet = await _petService.GetPetByIdAsync(id);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, pet));
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [Route("customer/add")]
        public async Task<IActionResult> AddPet([FromBody] PetRequestDto dto)
        {
            var ownerId = User.GetUserId();

            await _petService.CreatePetAsync(dto, ownerId);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsPet.ADD_PET_SUCCESS, null));
        }

        [HttpPut]
        [Route("customer/update")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdatePet([FromBody] PetUpdateRequestDto dto)
        {
            var ownerId = User.GetUserId();

            await _petService.UpdatePetAsync(dto, ownerId);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsPet.UPDATE_PET_SUCCESS, null));
        }

        [HttpDelete]
        [Authorize(Roles = "Customer")]
        [Route("customer/remove/{id:int}")]
        public async Task<IActionResult> RemovePet(int id)
        {
            var ownerId = User.GetUserId();

            await _petService.DeletePetAsync(id, ownerId);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsPet.DELETE_PET_SUCCESS, id));
        }
    }
}
