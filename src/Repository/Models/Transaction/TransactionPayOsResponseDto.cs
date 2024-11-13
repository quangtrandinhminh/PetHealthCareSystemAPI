namespace Repository.Models.Transaction;

public class TransactionPayOsResponseDto
{
    public long OrderId { get; set; }
    public string CheckoutUrl { get; set; }
}