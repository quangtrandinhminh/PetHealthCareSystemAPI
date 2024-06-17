using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO.Transaction;

public class TransactionMedicalItemsDto
{
    [Required]
    public int MedicalItemId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}