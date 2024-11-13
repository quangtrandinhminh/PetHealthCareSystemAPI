using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories;

public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
{
    private readonly AppDbContext _context = new();

    public async Task DeleteAppointmentAsync(int appointmentId)
    {
        var appointment = await _context.Appointments
            .Include(a => a.AppointmentPets)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);

        if (appointment != null)
        {
            _context.AppointmentPets.RemoveRange(appointment.AppointmentPets);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Appointment>> GetAppointmentsAsync()
    {
        using var context = new AppDbContext();
        var dbSet = context.Set<Appointment>();
        return await dbSet.AsQueryable().AsNoTracking().Include(e => e.Services).Include(e => e.AppointmentPets).ThenInclude(e => e.Pet).ToListAsync();
    }

    public async Task<Appointment> AddAppointmentAsync(Appointment appointment)
    {
        try
        {
            await using var db = new AppDbContext();

            // Ensure the tags exist and avoid duplicates
            foreach (var service in appointment.Services.ToList())
            {
                var existingService = db.Services.SingleOrDefault(e => e.Id == service.Id);

                if (existingService != null)
                {
                    // Attach the existing tag
                    db.Entry(existingService).State = EntityState.Unchanged;
                    appointment.Services.Remove(service);
                    appointment.Services.Add(existingService);
                }
            }

            var addedAppointment = await db.Appointments.AddAsync(appointment);
            await db.SaveChangesAsync();

            return addedAppointment.Entity;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}