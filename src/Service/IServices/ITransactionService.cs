using BusinessObject.DTO.Transaction;

namespace Service.IServices;

public interface ITransactionService
{
    TransactionDropdownDto GetTransactionDropdownData();
    Task<IList<TransactionResponseDto>> GetAllTransactionsAsync();
    Task<IList<TransactionResponseDto>> GetTransactionsByCustomerIdAsync(int customerId);
    Task<TransactionResponseWithDetailsDto> GetTransactionByIdAsync(int id);
    Task CreateTransactionAsync(TransactionRequestDto dto, int userId);
    //Task UpdateTransactionAsync(TransactionUpdateRequestDto transaction);
    //Task DeleteTransactionAsync(int id);
}