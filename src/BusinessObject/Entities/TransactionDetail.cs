using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class TransactionDetail
{
    public int Id { get; set; }

    public int? TransactionId { get; set; }

    public int? ServiceId { get; set; }

    public int? MedicalItemId { get; set; }

    public int? Quantity { get; set; }

    public int? SubTotal { get; set; }

    public bool? IsActive { get; set; }

    public virtual MedicalItem? MedicalItem { get; set; }

    public virtual Service? Service { get; set; }

    public virtual Transaction? Transaction { get; set; }
}
