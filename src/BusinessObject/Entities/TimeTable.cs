﻿using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Entities.Base;

namespace BusinessObject.Entities;

[Table("TimeTable")]
public class TimeTable : BaseEntity
{
    public int VetID { get; set; }
    public DateTimeOffset DateTimeStart { get; set; }
    public DateTimeOffset DateTimeEnd { get; set; }
    public ICollection<DayOfWeek> DayOfWeeks { get; set; }
    public string? Note { get; set; }
    
    [ForeignKey(nameof(VetID))]
    public virtual User Vet { get; set; }
    
    public virtual ICollection<Hospitalization>? Hospitalizations { get; set; }
    public virtual ICollection<Appointment>? Appointments { get; set; }
}