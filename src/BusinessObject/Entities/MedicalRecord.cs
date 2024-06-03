using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Entities;

[Table("MedicalRecord")]
public class MedicalRecord : BaseEntity
{
    public string PetId { get; set; }
    public string? HospitalizationId { get; set; }
    public string? RecordDetails { get; set; }
    public DateTimeOffset Date { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Note { get; set; }
    public DateTimeOffset? NextAppointment { get; set; }
    public decimal PetWeight { get; set; }
    
    [ForeignKey(nameof(PetId))]
    public virtual Pet Pet { get; set; }
    
    [ForeignKey(nameof(HospitalizationId))]
    public virtual Hospitalization? Hospitalization { get; set; }
    
    public virtual ICollection<MedicalItem>? MedicalItems { get; set; }
}