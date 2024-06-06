using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Appointment
{
    public int Id { get; set; }

    public int? PetId { get; set; }

    public int? TimetableId { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public string? BookingType { get; set; }

    public short? Rating { get; set; }

    public string? Feedback { get; set; }

    public bool? HasMedicalRecord { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();

    public virtual Pet? Pet { get; set; }

    public virtual Timetable? Timetable { get; set; }

    public virtual Transaction? Transaction { get; set; }
}
