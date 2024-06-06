using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class AppointmentService
{
    public int Id { get; set; }

    public int? AppointmentId { get; set; }

    public int? ServiceId { get; set; }

    public bool? IsActive { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Service? Service { get; set; }
}
