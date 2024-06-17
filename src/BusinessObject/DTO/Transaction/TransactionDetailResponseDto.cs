using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO.Transaction;

public class TransactionDetailResponseDto
{
    public int TransactionId { get; set; }
    public int? ServiceId { get; set; }
    public int? MedicalItemId { get; set; }
    public string? Name { get; set; } 
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SubTotal { get; set; }
}