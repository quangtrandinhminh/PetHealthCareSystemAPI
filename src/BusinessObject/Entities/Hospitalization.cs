using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Hospitalization
{
    public int Id { get; set; }

    public int? PetId { get; set; }

    public int? TimetableId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Diagnosis { get; set; }

    public string? Treatment { get; set; }

    public string? HospitalizationDetails { get; set; }

    public bool? IsActive { get; set; }

    public virtual Pet? Pet { get; set; }

    public virtual Timetable? Timetable { get; set; }
}
