using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Cage
{
    public int Id { get; set; }

    public int? PetId { get; set; }

    public string? Type { get; set; }

    public string? CageNumber { get; set; }

    public bool? IsActive { get; set; }

    public virtual Pet? Pet { get; set; }
}
