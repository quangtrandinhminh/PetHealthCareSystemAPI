using BusinessObject.DTO;
using BusinessObject.DTO.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHealthCareSystemAPI.Extensions;
using Service.IServices;
using Utility.Constants;
using Utility.Enum;
using Utility.Helpers;

namespace PetHealthCareSystemAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionController : Controller
{
    
    private ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    [Route("transactions")]
    public async Task<IActionResult> GetAllTransaction()
    {
        var response = await _transactionService.GetAllTransactionsAsync();
        return Ok(BaseResponseDto.OkResponseDto(response));
    }

    [HttpGet]
    [Route("dropdown-data")]
    public IActionResult GetTransactionDropdownData()
    {
        var response = _transactionService.GetTransactionDropdownData();
        return Ok(BaseResponseDto.OkResponseDto(response));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTransactionById(int id)
    {
        var response = await _transactionService.GetTransactionByIdAsync(id);
        return Ok(BaseResponseDto.OkResponseDto(response));
    }

    [HttpGet]
    [Route("customer/{customerId:int}/transactions")]
    public async Task<IActionResult> GetTransactionsByCustomerId(int customerId)
    {
        var response = await _transactionService.GetTransactionsByCustomerIdAsync(customerId);
        return Ok(BaseResponseDto.OkResponseDto(response));
    }

    [HttpPost]
    [Authorize(Roles = "Customer, Staff")]
    [Route("create")]
    public async Task<IActionResult> PostAsync([FromBody] TransactionRequestDto dto)
    {
        var userId = User.GetUserId();
        await _transactionService.CreateTransactionAsync(dto, userId);

        return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsTransaction.ADD_TRANSACTION_SUCCESS));
    }
}