using BusinessObject.Entities;

namespace Repository.Interfaces;

public interface IPetRepository
{
    IQueryable<Pet> GetPetsByUserId(string userId);
    void CreatePet(Pet pet);
    void UpdatePet(Pet pet);
    Pet FindPetById(int id);
}