using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class MedicalItem
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? UnitPrice { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
}
