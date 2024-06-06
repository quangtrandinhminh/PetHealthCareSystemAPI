using BusinessObject.DTO;
using BusinessObject.DTO.Pet;
using BusinessObject.Entities;

namespace Service.IServices;

public interface IPetService
{
    BaseResponseDto GetPetsByUserId(string userId);
    BaseResponseDto CreatePetForCustomer(PetCreateRequestDto petDto);
    BaseResponseDto UpdatePet(PetUpdateRequestDto petDto);
}