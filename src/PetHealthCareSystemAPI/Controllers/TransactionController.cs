using BusinessObject.DTO;
using BusinessObject.DTO.Transaction;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Utility.Constants;

namespace PetHealthCareSystemAPI.Controllers;

public class TransactionController : Controller
{
    private ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    [Route("get-all-transactions")]
    public async Task<IActionResult> GetAllTransaction()
    {
        var response = await _transactionService.GetAllTransactionsAsync();
        return Ok(BaseResponseDto.OkResponseDto(response));
    }

    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    [HttpPost]
    [Route("Create Transaction")]
    public async Task<OkObjectResult> PostAsync([FromBody] TransactionResponseDto dto)
    {
        //await _transactionService.CreateTransactionAsync(dto);

        return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsPet.ADD_PET_SUCCESS));
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}