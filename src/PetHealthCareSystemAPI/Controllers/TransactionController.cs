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
    [Route("get-all")]
    public async Task<IActionResult> GetAllTransaction([FromQuery] int pageNumber = 1, int pageSize = 10)
    {
        var response = await _transactionService.GetAllTransactionsAsync(pageNumber, pageSize);
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
    [Route("customer/your-transactions")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetTransactionsByCustomerId([FromQuery] int pageNumber = 1, int pageSize = 10)
    {
        var customerId = User.GetUserId();
        var response = await _transactionService.
            GetTransactionsByCustomerIdAsync(customerId, pageNumber, pageSize);
        return Ok(BaseResponseDto.OkResponseDto(response));
    }

    [HttpGet]
    [Route("appointment/{id:int}")]
    public async Task<IActionResult> GetTransactionByAppointmentId(int id)
    {
        var response = await _transactionService.GetTransactionByAppointmentIdAsync(id);
        return Ok(BaseResponseDto.OkResponseDto(response));
    }

    [HttpGet]
    [Route("medical-record/{id:int}")]
    public async Task<IActionResult> GetTransactionByMedicalRecordId(int id)
    {
        var response = await _transactionService.GetTransactionByMedicalRecordIdAsync(id);
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

    [HttpGet]
    [Route("hospitalization-price/{id:int}")]
    public async Task<IActionResult> CalculateHospitalizationPrice(int id)
    {
        var response = await _transactionService.CalculateHospitalizationPriceAsync(id);
        return Ok(BaseResponseDto.OkResponseDto(response));
    }

    [HttpPost]
    [Authorize(Roles = "Staff")]
    [Route("hospitalization/create")]
    public async Task<IActionResult> CreateTransactionForHospitalization([FromBody] TransactionRequestDto dto)
    {

        var userId = User.GetUserId();
        await _transactionService.CreateTransactionForHospitalization(dto, userId);

        return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsTransaction.ADD_TRANSACTION_SUCCESS));

    }

    [HttpPut]
    [Authorize(Roles = "Staff")]
    [Route("staff/update-payment/{id:int}")]
    public async Task<IActionResult> UpdatePaymentByStaff(int id)
    {
        var userId = User.GetUserId();
        await _transactionService.UpdatePaymentByStaffAsync(id, 1);

        return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsTransaction.UPDATE_PAYMENT_SUCCESS));
    }

    [HttpPut]
    [Authorize(Roles = "Staff, Customer")]
    [Route("staff/update-refund")]
    public async Task<IActionResult> UpdateTransactionToRefund([FromBody] TransactionRefundRequestDto dto)
    {
        var userId = User.GetUserId();
        await _transactionService.UpdateTransactionToRefundAsync(dto, userId);

        return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsTransaction.UPDATE_REFUND_SUCCESS));
    }

    [HttpGet]
    [Route("refund-conditions")]
    public async Task<IActionResult> GetRefundPercentage()
    {
        var response = await _transactionService.GetRefundConditionsAsync();
        return Ok(BaseResponseDto.OkResponseDto(response));
    }
}