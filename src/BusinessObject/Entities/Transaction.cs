using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Transaction
{
    public int Id { get; set; }

    public string? CustomerId { get; set; }

    public int? AppointmentId { get; set; }

    public int? Total { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? Status { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentNote { get; set; }

    public double? RefundPercentage { get; set; }

    public string? RefundReason { get; set; }

    public DateTime? RefundDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual User? Customer { get; set; }

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
}
