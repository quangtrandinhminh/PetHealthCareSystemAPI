using BusinessObject.DTO.Pet;

namespace Service.IServices;

public interface IPetService
{
    Task<List<PetResponseDto>> GetAllPetsForCustomerAsync(int id);
    Task CreatePetAsync(PetRequestDto pet);
    Task UpdatePetAsync(PetRequestDto pet);
    Task DeletePetAsync(int id, int deleteBy);
}