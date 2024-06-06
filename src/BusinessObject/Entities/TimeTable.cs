using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Timetable
{
    public int Id { get; set; }

    public string? VetId { get; set; }

    public string? Note { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Hospitalization> Hospitalizations { get; set; } = new List<Hospitalization>();

    public virtual User? Vet { get; set; }
}
