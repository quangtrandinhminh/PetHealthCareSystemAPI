using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories;

public class PetRepository : BaseRepository<Pet>, IPetRepository
{
    public async Task<List<Pet>> GetAllPetsByCustomerIdAsync(int id)
    {
        var list = GetAll().Include(e => e.Owner);
        return await list.Where(e => e.OwnerID == id && e.DeletedBy == null).ToListAsync();
    }

    public async Task UpdatePetAsync(Pet pet)
    {
        await UpdateAsync(pet);
    }

    public async Task DeletePetAsync(Pet pet)
    {
        await UpdatePetAsync(pet);
    }

    public async Task CreatePetAsync(Pet pet)
    {
        await AddAsync(pet);
    }

    private static readonly AppDbContext _context = new();

    public async Task DeletePetAsync(int petId)
    {
        var pet = await _context.Pets
            .Include(p => p.AppointmentPets)
            .FirstOrDefaultAsync(p => p.Id == petId);

        if (pet != null)
        {
            _context.AppointmentPets.RemoveRange(pet.AppointmentPets);
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
        }
    }
}