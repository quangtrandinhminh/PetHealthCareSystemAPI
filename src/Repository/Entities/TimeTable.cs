using System.ComponentModel.DataAnnotations.Schema;
using Repository.Entities.Base;
using Utility.Enum;

namespace Repository.Entities;

[Table("TimeTable")]
public class TimeTable : BaseEntity
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public TimeTableType Type { get; set; }
    public virtual ICollection<Hospitalization>? Hospitalizations { get; set; }
    public virtual ICollection<Appointment>? Appointments { get; set; }
}