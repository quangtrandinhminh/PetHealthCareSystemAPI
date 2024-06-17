using System.ComponentModel.DataAnnotations;
using Utility.Enum;

namespace BusinessObject.DTO.Transaction;

public class TransactionRequestDto
{
    public int? AppointmentId { get; set; }
    public int? MedicalRecordId { get; set; }
    public int? HospitalizationId { get; set; }

    [Required]
    public int PaymentMethod { get; set; }
    public DateTimeOffset? PaymentDate { get; set; }

    [Required]
    public int Status { get; set; }
    public List<TransactionServicesDto>? Services { get; set; }
    public List<TransactionMedicalItemsDto>? MedicalItems { get; set; }
}