using Repository.Models;

namespace Repository.Interfaces;

public interface IAppointmentPetRepository
{
    Task CreateAsync(AppointmentPet appointmentPet);
}