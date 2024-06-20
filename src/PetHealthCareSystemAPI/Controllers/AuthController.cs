using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using BusinessObject.DTO;
using BusinessObject.DTO.RefreshToken;
using BusinessObject.DTO.User;
using BusinessObject.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHealthCareSystemAPI.Auth;
using Service.IServices;
using Service.Services;
using Utility.Constants;
using Utility.Enum;

namespace PetHealthCareSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authSevices)
        {
            _authService = authSevices;
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            await _authService.Register(request);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageIdentitySuccess.REGIST_USER_SUCCESS));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("admin/register")]
        public async Task<IActionResult> RegisterByAdmin([FromBody] RegisterDto request, int role)
        {
            await _authService.RegisterByAdmin(request, role);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageIdentitySuccess.REGIST_USER_SUCCESS));
        }

        
        [HttpPost]
        [AllowAnonymous]
        [Route("authenticate")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            return Ok(await _authService.Authenticate(request));
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken(RefreshTokenDto request)
        {
            var refreshToken = request.Token ?? Request.Cookies["refreshToken"];
            var response = await _authService.RefreshToken(refreshToken);
            SetTokenCookie(response.RefreshToken);
            return Ok(BaseResponseDto.OkResponseDto(response));
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDto request)
        {
            await _authService.VerifyEmail(request);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageIdentitySuccess.VERIFY_EMAIL_SUCCESS));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto request)
        {
            await _authService.ForgotPassword(request);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageIdentitySuccess.FORGOT_PASSWORD_SUCCESS));
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto request)
        {
            await _authService.ChangePassword(request);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageIdentitySuccess.CHANGE_PASSWORD_SUCCESS));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto request)
        {
            await _authService.ResetPassword(request);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageIdentitySuccess.RESET_PASSWORD_SUCCESS));
        }

        [HttpPost("resend-email")]
        public async Task<IActionResult> ResendEmail(ResendEmailDto request)
        {
            await _authService.ReSendEmail(request);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageIdentitySuccess.RESEND_EMAIL_SUCCESS));
        }

        [HttpGet("test-author")]
        [Authorize(Roles = "Admin")]
        public IActionResult TestAuthorize()
        {
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("StaffRegister")]
        public async Task<IActionResult> StaffRegister([FromBody] RegisterDto request)
        {
            await _authService.StaffRegistor(request);
            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageIdentitySuccess.REGIST_USER_SUCCESS));
        }
    }
}