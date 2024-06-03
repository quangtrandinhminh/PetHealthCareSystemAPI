using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Entities;

public class Hospitalization : BaseEntity
{
    public string MedicalRecordId { get; set; }
    public string? CageId { get; set; }
    public string? VetId { get; set; }
    public DateTimeOffset AdmitDate { get; set; }
    public DateTimeOffset? DischargeDate { get; set; }
    public string? Reason { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Note { get; set; }
    
    [ForeignKey(nameof(MedicalRecordId))]
    public virtual MedicalRecord MedicalRecord { get; set; }
    
    [ForeignKey(nameof(CageId))]
    public virtual Cage? Cage { get; set; }
    
    [ForeignKey(nameof(VetId))]
    public virtual User? Vet { get; set; }
}