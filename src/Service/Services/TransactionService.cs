using BusinessObject.DTO.Transaction;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using Serilog;
using Service.IServices;
using Utility.Constants;
using Utility.Exceptions;

namespace Service.Services;

public class TransactionService(IServiceProvider serviceProvider) : ITransactionService
{
    private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
    private readonly RoleManager<RoleEntity> _roleManager = serviceProvider.GetRequiredService<RoleManager<RoleEntity>>();
    private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
    private readonly ILogger _logger = Log.Logger;
    private readonly SignInManager<UserEntity> _signInManager = serviceProvider.GetRequiredService<SignInManager<UserEntity>>();
    private readonly ITransactionRepository _transactionRepository = serviceProvider.GetRequiredService<ITransactionRepository>();
    private readonly ITransactionDetailRepository _transactionDetailRepository = serviceProvider.GetRequiredService<ITransactionDetailRepository>();
    public async Task<IList<TransactionResponseDto>> GetAllTransactionsAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();
        if (transactions == null || transactions.Count == 0)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsTransaction.TRANSACTION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(transactions);
        return response;
    }
}