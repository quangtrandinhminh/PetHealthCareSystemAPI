using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using BusinessObject.DTO;
using BusinessObject.DTO.User;
using BusinessObject.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.IServices;
using Service.Services;
using Utility.Constants;
using Utility.Enum;

namespace PetHealthCareSystemAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userSevices, ITokenService tokenService)
        {
            _userService = userSevices;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult TestAuthor()
        {
            return Ok("This is admin data");
        }

        [HttpPost]
        [Route("register"), AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            await _userService.Register(registerDto);
            return Ok(BaseResponseDto.OkResponseModel(ReponseMessageIdentity.REGIST_USER_SUCCESS));
        }

        [HttpPost]
        [Route("authenticate"), AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            return Ok(await _userService.Authenticate(loginDto));
        }
    }
}