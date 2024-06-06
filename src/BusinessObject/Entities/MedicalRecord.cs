using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class MedicalRecord
{
    public int Id { get; set; }

    public int? PetId { get; set; }

    public string? VetId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Diagnosis { get; set; }

    public string? Treatment { get; set; }

    public string? RecordDetails { get; set; }

    public bool? IsActive { get; set; }

    public virtual Pet? Pet { get; set; }

    public virtual User? Vet { get; set; }
}
