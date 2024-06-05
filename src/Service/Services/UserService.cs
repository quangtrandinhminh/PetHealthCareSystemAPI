using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BusinessObject.DTO.User;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Repository.Interface;
using Repository.Repositories;
using Serilog;
using Service.IServices;
using Service.Utils;
using Utility.Constants;
using Utility.Exceptions;
using Utility.Helpers;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly MapperlyMapper _mapper;
        private readonly RoleManager<RoleEntity> _roleManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly ILogger _logger;

        public UserService(IServiceProvider serviceProvider)
        {
            _logger = Log.Logger;
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<RoleEntity>>();
            _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
        }

        public async Task Register(RegisterDto dto)
        {
            var validateUser = await _userRepository.GetUserByUserName(dto.UserName);
            if (validateUser != null)
            {
                throw new AppException(ResponseCodeConstants.EXISTED, ReponseMessageIdentity.EXISTED_USER, StatusCodes.Status400BadRequest);
            }

            var existingUserWithEmail = await _userRepository.GetUserByEmail(dto.Email);
            if (existingUserWithEmail != null)
            {
                throw new AppException(ResponseCodeConstants.EXISTED, ReponseMessageIdentity.EXISTED_EMAIL, StatusCodes.Status400BadRequest);
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                throw new AppException(ResponseCodeConstants.INVALID_INPUT, ReponseMessageIdentity.PASSWORD_NOT_MATCH, StatusCodes.Status400BadRequest);
            }

            if (!string.IsNullOrEmpty(dto.PhoneNumber) && !Regex.IsMatch(dto.PhoneNumber, @"^\d{10}$"))
            {
                throw new AppException(ResponseCodeConstants.INVALID_INPUT, ReponseMessageIdentity.PHONENUMBER_INVALID, StatusCodes.Status400BadRequest);
            }

            try
            {
                var account = _mapper.Map(dto);
                account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                await _userRepository.CreateAsync(account);

                await _userRepository.AddUserToRoleAsync(account, 4);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            // send sms to phone number here
        }

        public async Task<UserResponseDto> Authenticate(LoginDto dto)
        {
            var account = await _userRepository.GetUserByUserName(dto.Username);
            if (account == null || account.DeletedTime != null) throw new AppException(ErrorCode.UserInvalid, ReponseMessageIdentity.INVALID_USER, StatusCodes.Status401Unauthorized);

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, account.PasswordHash))
            {
                throw new AppException(ErrorCode.UserPasswordWrong, ReponseMessageIdentity.PASSWORD_WRONG, StatusCodes.Status401Unauthorized);
            }

            var role = await _userRepository.GetUserRoleByUsernameAsync(dto.Username);
            var token = await GenerateJWTToken(account, role, 1);
            var refreshToken = GenerateRefreshToken(account.Id, 12);
            var response = _mapper.Map(account);
            response.Role = role;
            response.Token = token;
            response.RefreshToken = refreshToken.Token;
            return response;
        }

        public async Task<UserEntity> GetUserByUsernameAndPassword(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAndPassword(username, password);

            return user;
        }

        public async Task<string> GetUserRoleByUsernameAsync(string username)
        {
            var role = await _userRepository.GetUserRoleByUsernameAsync(username);

            return role;
        }


        private async Task<string> GenerateJWTToken(UserEntity loggedUser, string role, int hour)
        {
            var claims = new List<Claim>();
            claims.AddRange(await _userManager.GetClaimsAsync(loggedUser));
            var userRole = await _roleManager.FindByNameAsync(role);
            if (userRole != null) claims.AddRange(await _roleManager.GetClaimsAsync(userRole));

            claims.AddRange(new[]
            {
                new Claim(ClaimTypes.Sid, loggedUser.Id.ToString()),
                new Claim("UserName", loggedUser.UserName),
                new Claim(ClaimTypes.Name, loggedUser.FullName),
                new Claim(ClaimTypes.Email, loggedUser.Email),
                new Claim(ClaimTypes.MobilePhone, loggedUser.PhoneNumber),
                new Claim(ClaimTypes.Expired, CoreHelper.SystemTimeNow.AddHours(hour).Date.ToShortDateString())
            });

            return JwtUtils.GenerateToken(claims.Distinct(), hour);
        }

        private static RefreshToken GenerateRefreshToken(int userId, int hour)
        {
            var randomByte = new byte[64];
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            rngCryptoServiceProvider.GetBytes(randomByte);
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = Convert.ToBase64String(randomByte),
                Expires = CoreHelper.SystemTimeNow.AddHours(hour),
            };
            return refreshToken;
        }
    }
}