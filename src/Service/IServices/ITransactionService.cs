using BusinessObject.DTO.Transaction;
using Net.payOS.Types;
using Repository.Extensions;

namespace Service.IServices;

public interface ITransactionService
{
    TransactionDropdownDto GetTransactionDropdownData();
    Task<PaginatedList<TransactionResponseDto>> GetAllTransactionsAsync(int pageNumber, int pageSize);
    Task<PaginatedList<TransactionResponseDto>> GetTransactionsByCustomerIdAsync(int customerId, int pageNumber,
        int pageSize);
    Task<TransactionResponseDto> GetTransactionByAppointmentIdAsync(int appointmentId);
    Task<TransactionResponseDto> GetTransactionByMedicalRecordIdAsync(int medicalRecordId);
    Task<TransactionResponseWithDetailsDto> GetTransactionByIdAsync(int transactionId);
    Task<TransactionPayOsResponseDto> CreateTransactionAsync(TransactionRequestDto dto, int userId);
    Task CreateTransactionForHospitalization(TransactionRequestDto dto, int staffId);
    Task UpdatePaymentByStaffAsync(int transactionId, int updatedById);
    Task UpdateTransactionToRefundAsync(TransactionRefundRequestDto dto, int updatedById);
    Task<RefundConditionsResponseDto> GetRefundConditionsAsync();
    Task<TransactionPayOsResponseDto> CreatePayOsTransaction(List<ItemData> items, int totalAmount, string payDescription, int transactionId);
    Task<HospitalizationPriceResponseDto> CalculateHospitalizationPriceAsync(
        int medicalRecordId);
}