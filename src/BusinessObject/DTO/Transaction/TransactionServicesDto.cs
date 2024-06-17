using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO.Transaction;

public class TransactionServicesDto
{
    [Required]
    public int ServiceId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}