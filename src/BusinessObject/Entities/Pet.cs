using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Pet
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Species { get; set; }

    public string? Breed { get; set; }

    public int? Age { get; set; }

    public string? OwnerId { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Cage? Cage { get; set; }

    public virtual ICollection<Hospitalization> Hospitalizations { get; set; } = new List<Hospitalization>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual User? Owner { get; set; }
}
