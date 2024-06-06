using BusinessObject.Entities;
using DataAccessLayer.DAO;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository.Repositories;

public class PetRepository: IPetRepository
{
    public Pet FindPetById(int id)
    {
        return PetDao.FindByCondition(e => e.Id == id).FirstOrDefault();
    }

    public IQueryable<Pet> GetPetsByUserId(string userId)
    {
        var pets = PetDao.GetAll().Include(e => e.Owner).Where(e => e.OwnerId.Equals(userId)).AsQueryable();

        return pets;
    }

    public void CreatePet(Pet pet)
    {
        PetDao.Add(pet);
    }

    public void UpdatePet(Pet pet)
    {
        PetDao.Update(pet);
    }
}