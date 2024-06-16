using BusinessObject.DTO.Transaction;

namespace Service.IServices;

public interface ITransactionService
{
    Task<IList<TransactionResponseDto>> GetAllTransactionsAsync();
    //Task<List<TransactionResponseDto>> GetTransactionsByCustomerIdAsync(int customerId);
    //Task<TransactionResponseDto> GetTransactionByIdAsync(int id);
    //Task CreateTransactionAsync(TransactionRequestDto transaction);
    //Task UpdateTransactionAsync(TransactionUpdateRequestDto transaction);
    //Task DeleteTransactionAsync(int id);
}