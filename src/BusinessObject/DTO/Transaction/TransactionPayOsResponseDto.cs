namespace BusinessObject.DTO.Transaction;

public class TransactionPayOsResponseDto
{
    public int TransactionId { get; set; }
    public long OrderId { get; set; }    
    public string CheckoutUrl { get; set; }
}