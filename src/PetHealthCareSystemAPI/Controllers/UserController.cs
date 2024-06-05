using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.DTO.User;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Classes;
using Service.Interfaces;
using Utility.Enum;

namespace PetHealthCareSystemAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signinManager;

        public UserController(UserManager<User> userManager, ITokenService tokenService,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signInManager;
            _userService = new UserService(_userManager, _signinManager);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult TestAuthor()
        {
            return Ok("This is admin data");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var createdUser = await _userService.CreateCustomerAsync(user, registerDto.Password);

            if (createdUser.Succeeded)
            {
                return Ok(
                    new NewUserDto
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        Token = _tokenService.CreateToken(user)
                    });
            }
            else
            {
                return StatusCode(500, createdUser.Errors);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetUserByUsernameAndPassword(loginDto.Username, loginDto.Password);

            if (user == null)
            {
                return BadRequest("Incorrect username or password");
            }

            return Ok(
                new ExistingUserDto()
                {
                    Address = user.Address,
                    Avatar = user.Avatar,
                    BirthDate = user.BirthDate,
                    Email = user.Email,
                    FullName = user.FullName,
                    Token = _tokenService.CreateToken(user),
                    Username = user.UserName,
                    Role = await _userService.GetUserRoleByUsernameAsync(user.UserName)
                }
            );
        }
    }
}