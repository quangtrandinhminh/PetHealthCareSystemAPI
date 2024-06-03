using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utility.Enum;

namespace BusinessObject.Entities;

[Table("Transaction")]
public class Transaction : BaseEntity
{
    protected Transaction()
    {
        Status = TransactionStatus.Pending;
    }
    
    // Payment
    public string CustomerId { get; set; }
    public string? AppointmentId { get; set; }
    public string? MedicalRecordId { get; set; }
    
    public string? HospitalizationId { get; set; }
    
    [Column(TypeName = "decimal(18, 0)")]
    [Range(0, Double.MaxValue)]
    public decimal Total { get; set; }
    public DateTimeOffset? PaymentDate { get; set; }
    public TransactionStatus Status { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public string? PaymentNote { get; set; }
    public string? PaymentId { get; set; }
    public string? PaymentStaffName { get; set; }
    public string? Note { get; set; }
    
    // Refund
    [Column(TypeName = "decimal(5, 2)")]
    [Range(0, 1)]
    public  decimal? RefundPercentage { get; set; }
    public string? RefundReason { get; set; }
    public DateTimeOffset? RefundDate { get; set; }
    
    [ForeignKey(nameof(CustomerId))]
    public User Customer { get; set; }
    
    // pay for appointment in the first time
    [ForeignKey(nameof(AppointmentId))]
    public Appointment? Appointment { get; set; }
    
    // pay for medical items or hospitalization in medical record
    [ForeignKey(nameof(MedicalRecordId))]
    public MedicalRecord? MedicalRecord { get; set; }
    
    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
}