using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Repository.Models.VNPay;
using Service.IServices;

namespace PetHealthCareSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _paymentService;

        public PaymentController(IVnPayService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        [Route("vnpay/payment-link")]
        public IActionResult CreatePayment([FromBody] VnPaymentRequestDto request)
        {
            var response = _paymentService.CreatePaymentUrl(request);
            return Ok(BaseResponseDto.OkResponseDto(response));
        }

        [HttpPost]
        [Route("vnpay/payment-execute")]
        public IActionResult VerifyPayment([FromBody] HttpContext request)
        {
            var response = _paymentService.PaymentExecute(request);
            return Ok(BaseResponseDto.OkResponseDto(response));
        }
    }
}
