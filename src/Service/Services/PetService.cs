using BusinessObject.DTO;
using BusinessObject.DTO.Pet;
using BusinessObject.Entities;
using BusinessObject.Mappers;
using Repository.Interfaces;
using Service.IServices;

namespace Service.Services;

public class PetService : IPetService
{
    private readonly IRepositoryManager _repository;

    public PetService(IRepositoryManager repositoryManager)
    {
        _repository = repositoryManager;
    }

    public BaseResponseDto GetPetsByUserId(string userId)
    {
        var pets = _repository.Pet.GetPetsByUserId(userId).Select(e => e.ToPetDto());

        return new BaseResponseDto(200, "Success", pets, "No additional data", "Get successfully");
    }

    public BaseResponseDto CreatePetForCustomer(PetCreateRequestDto petDto)
    {
        _repository.Pet.CreatePet(petDto.ToPetFromCreateRequestDto());

        return new BaseResponseDto(200, "Success", petDto, "No additional data", "Create successfully");
    }

    public BaseResponseDto UpdatePet(PetUpdateRequestDto petDto)
    {
        var existingPet = _repository.Pet.FindPetById(petDto.Id);

        if (existingPet == null)
        {
            return new BaseResponseDto(400, "Failing", "Not found pet in the data source");
        }

        var updatePet = petDto.ToPetFromUpdateRequestDto(existingPet);

        _repository.Pet.UpdatePet(updatePet);

        return new BaseResponseDto(200, "Success", updatePet.ToPetDto(), "No additional data", "Update successfully");
    }
}