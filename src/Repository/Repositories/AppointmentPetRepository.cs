using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Repository.Models;

namespace Repository.Repositories;

public class AppointmentPetRepository : IAppointmentPetRepository
{
    public async Task CreateAsync(AppointmentPet entity)
    {
        await using var context = new AppDbContext();
        var dbSet = context.Set<AppointmentPet>();
        await dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        context.Entry(entity).State = EntityState.Detached;
    }
}