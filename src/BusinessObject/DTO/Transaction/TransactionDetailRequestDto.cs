namespace BusinessObject.DTO.Transaction;

public class TransactionDetailRequestDto
{
    public Dictionary<int, int>? ServiceIdsMap { get; set; }
    public Dictionary<int, int>? MedicalItemIdsMap { get; set; }
    public decimal? Discount { get; set; }
    public string? Note { get; set; }
}