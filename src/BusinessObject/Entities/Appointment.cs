using System.ComponentModel.DataAnnotations.Schema;
using Utility.Enum;

namespace BusinessObject.Entities;

[Table("Appointment")]
public class Appointment : BaseEntity
{
    public string VetId { get; set; }
    public DateTimeOffset Date { get; set; }
    public string? Note { get; set; }
    public AppointmentStatus Status { get; set; }
    public AppointmentBookingType BookingType { get; set; }
    public short? Rating { get; set; }
    public string? Feedback { get; set; }

    [ForeignKey(nameof(VetId))] 
    public virtual User Vet { get; set; }
    
    public virtual ICollection<Pet> Pets { get; set; }

    public virtual ICollection<Service> Services { get; set; }
}