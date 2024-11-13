using Microsoft.AspNetCore.Http;
using Repository.Models.VNPay;

namespace Service.IServices;

public interface IVnPayService
{
    string CreatePaymentUrl(VnPaymentRequestDto dto);
    VnPaymentResponseDto PaymentExecute(HttpContext context);
}