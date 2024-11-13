using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models;

public class AppointmentPet
{
    public int AppointmentId { get; set; }

    [ForeignKey(nameof(AppointmentId))]
    public virtual Entities.Appointment Appointment { get; set; }

    public int PetId { get; set; }

    [ForeignKey(nameof(PetId))]
    public virtual Entities.Pet Pet { get; set; }
}