using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Entities;

[Table("Pet")]
public class Pet : BaseEntity
{
    public string? Name { get; set; }
    public string? Species { get; set; }
    public string? Breed { get; set; }
    public int? Age { get; set; }

    public string OwnerID { get; set; }
    [ForeignKey(nameof(OwnerID))] 
    public virtual User User { get; set; }
    
    public virtual ICollection<MedicalRecord>? MedicalRecords { get; set; }
}