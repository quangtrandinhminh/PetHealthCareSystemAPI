using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Utility.Enum;

namespace BusinessObject.DTO.Transaction;

public class TransactionResponseDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int? AppointmentId { get; set; }
    public int? MedicalRecordId { get; set; }
    public int? HospitalizationId { get; set; }
    public decimal Total { get; set; }
    public DateTimeOffset? PaymentDate { get; set; }
    public TransactionStatus Status { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public string? PaymentNote { get; set; }
    public string? PaymentId { get; set; }
    public int? PaymentStaffId { get; set; }
    public string? PaymentStaffName { get; set; }
    public string? Note { get; set; }
    public decimal? RefundPercentage { get; set; }
    public string? RefundReason { get; set; }
    public DateTimeOffset? RefundDate { get; set; }
}