using System.Text.Json.Nodes;
using BusinessObject.DTO;
using BusinessObject.DTO.Transaction;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using Serilog;
using Service.IServices;
using Utility.Constants;
using Utility.Enum;
using Utility.Exceptions;

namespace Service.Services;

public class TransactionService(IServiceProvider serviceProvider) : ITransactionService
{
    private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
    private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
    private readonly ILogger _logger = Log.Logger;
    private readonly ITransactionRepository _transactionRepository = serviceProvider.GetRequiredService<ITransactionRepository>();
    private readonly IAppointmentRepository _appointmentRepository = serviceProvider.GetRequiredService<IAppointmentRepository>();
    private readonly IMedicalRecordRepository _medicalRecordRepository = serviceProvider.GetRequiredService<IMedicalRecordRepository>();
    private readonly IHospitalizationRepository _hospitalizationRepository = serviceProvider.GetRequiredService<IHospitalizationRepository>();
    private readonly IServiceRepository _serviceRepository = serviceProvider.GetRequiredService<IServiceRepository>();
    private readonly IMedicalItemRepository _medicalItemRepository = serviceProvider.GetRequiredService<IMedicalItemRepository>();

    public async Task<IList<TransactionResponseDto>> GetAllTransactionsAsync()
    {
        _logger.Information("Get all transactions");
        var transactions = await _transactionRepository.GetAllWithCondition(
            t => t.DeletedTime == null,
            t => t.Customer).ToListAsync();
        if (transactions == null || transactions.Count == 0)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsTransaction.TRANSACTION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(transactions);
        return response;
    }

    public async Task<IList<TransactionResponseDto>> GetTransactionsByCustomerIdAsync(int customerId)
    {
        _logger.Information("Get all transactions by customer id");
        var transactions = await _transactionRepository.GetAllWithCondition(t =>
            t.CustomerId == customerId && t.DeletedTime == null,
            t => t.Customer).ToListAsync() ;
        if (transactions == null || transactions.Count == 0)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsTransaction.TRANSACTION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(transactions);
        return response;
    }

    public async Task<TransactionResponseWithDetailsDto> GetTransactionByIdAsync(int id)
    {
        _logger.Information("Get transaction by id");

        var transaction = await _transactionRepository.GetSingleAsync(t => t.Id == id,
            false, t => t.TransactionDetails,
            t => t.Customer);
        if (transaction == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND
                , ResponseMessageConstantsTransaction.TRANSACTION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.TransactionToTransactionResponseWithDetails(transaction);
        response.TransactionDetails = _mapper.Map(transaction.TransactionDetails.ToList());
        var customer = await _userRepository.GetSingleAsync(u => u.Id == transaction.CustomerId);
        response.CustomerName = customer.FullName;
        return response;
    }

    // get all dropdown data for transaction
    public TransactionDropdownDto GetTransactionDropdownData()
    {
        _logger.Information("Get all dropdown data for transaction");
        var paymentMethods = Enum.GetValues(typeof(PaymentMethod))
            .Cast<PaymentMethod>()
            .Select(e => new EnumResponseDto() { Id = (int)e, Value = e.ToString() })
            .ToList();

        var transactionStatuses = Enum.GetValues(typeof(TransactionStatus))
            .Cast<TransactionStatus>()
            .Select(e => new EnumResponseDto { Id = (int)e, Value = e.ToString() })
            .ToList();

        var response = new TransactionDropdownDto
        {
            PaymentMethods = paymentMethods,
            TransactionStatus = transactionStatuses
        };

        return response;
    }

    public async Task CreateTransactionAsync(TransactionRequestDto dto, int userId)
    {
        _logger.Information("Create transaction {@dto}", dto);
        var userEntity = _userManager.FindByIdAsync(userId.ToString()).Result;
        if (userEntity == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageIdentity.INVALID_USER, StatusCodes.Status404NotFound);
        }

        /*if (dto.AppointmentId == null && dto.MedicalRecordId == null &&
            dto.HospitalizationId == null)
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST
                , ResponseMessageConstantsTransaction.INVALID_TRANSACTION, StatusCodes.Status400BadRequest);
        }*/

        if (dto.AppointmentId != null)
        {
            var appointment = await _appointmentRepository.GetSingleAsync(a => a.Id == dto.AppointmentId);
            if (appointment == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND
                    , ResponseMessageConstantsAppointment.APPOINTMENT_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        if (dto.MedicalRecordId != null)
        {
            var medicalRecord = await _medicalRecordRepository.GetSingleAsync(mr =>
                mr.Id == dto.MedicalRecordId);
            if (medicalRecord == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND
                    , ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        if (dto.HospitalizationId != null)
        {
            var hospitalization = await _hospitalizationRepository.GetSingleAsync(h =>
                h.Id == dto.HospitalizationId);
            if (hospitalization == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND,
                    ResponseMessageConstantsHospitalization.HOSPITALIZATION_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        if ((dto.MedicalItems == null || dto.MedicalItems.Count == 0) && (dto.Services == null || dto.Services.Count == 0))
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                ResponseMessageConstantsTransaction.TRANSACTION_DETAIL_REQUIRED,
                StatusCodes.Status400BadRequest);
        }

        var transactionEntity = _mapper.Map(dto);
        transactionEntity.CreatedBy = userEntity.Id;
        // use userManager to check if list of roles contains staff or customer
        var roles = await _userManager.GetRolesAsync(userEntity);
        if (roles.Contains(UserRole.Staff.ToString()))
        {
            transactionEntity.PaymentStaffId = userEntity.Id;
            transactionEntity.PaymentStaffName = userEntity.FullName;
        }
        else if (roles.Contains(UserRole.Customer.ToString()))
        {
            transactionEntity.CustomerId = userEntity.Id;
        }

        // for each service in list, create transaction detail
        transactionEntity.TransactionDetails = new List<TransactionDetail>();
        if (dto.Services != null)
        {
            foreach (var service in dto.Services)
            {
                var serviceEntity = await _serviceRepository.GetSingleAsync(s => s.Id == service.ServiceId);
                if (serviceEntity == null)
                {
                    throw new AppException(ResponseCodeConstants.NOT_FOUND,
                        ResponseMessageConstantsService.SERVICE_NOT_FOUND + $": {service.ServiceId}",
                        StatusCodes.Status404NotFound);
                }
                var transactionDetail = new TransactionDetail
                {
                    ServiceId = service.ServiceId,
                    Name = serviceEntity.Name,
                    Quantity = service.Quantity,
                    Price = serviceEntity.Price,
                    SubTotal = serviceEntity.Price * service.Quantity,
                    TransactionId = transactionEntity.Id,
                };
                transactionEntity.TransactionDetails.Add(transactionDetail);
                transactionEntity.Total += transactionDetail.SubTotal;
            }
        }
        

        // for each medical item in list, create transaction detail
        if (dto.MedicalItems != null)
        {
            foreach (var medicalItem in dto.MedicalItems)
            {
                var medicalItemEntity = await _medicalItemRepository.GetSingleAsync(m =>
                    m.Id == medicalItem.MedicalItemId);
                if (medicalItemEntity == null)
                {
                    throw new AppException(ResponseCodeConstants.NOT_FOUND,
                        ResponseMessageConstantsMedicalItem.MEDICAL_ITEM_NOT_FOUND + $": {medicalItem.MedicalItemId}",
                        StatusCodes.Status404NotFound);
                }
                var transactionDetail = new TransactionDetail
                {
                    MedicalItemId = medicalItem.MedicalItemId,
                    Name = medicalItemEntity.Name,
                    Quantity = medicalItem.Quantity,
                    Price = medicalItemEntity.Price,
                    SubTotal = medicalItemEntity.Price * medicalItem.Quantity,
                    TransactionId = transactionEntity.Id,
                };
                
                transactionEntity.TransactionDetails.Add(transactionDetail);
                transactionEntity.Total += transactionDetail.SubTotal;
            }
        }

        await _transactionRepository.AddAsync(transactionEntity);
    }
}