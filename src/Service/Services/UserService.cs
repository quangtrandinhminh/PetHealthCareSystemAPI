using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Identity;
using Repository.Interfaces;
using Repository.Repositories;
using Service.IServices;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(UserManager<User> userManager, SignInManager<User> signinManager)
        {
            _userRepo = new UserRepository(userManager, signinManager);
        }

        public async Task<IdentityResult> CreateCustomerAsync(User user, string password)
        {
            var result = await _userRepo.CreateCustomerAsync(user, password);

            return result;
        }

        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            var user = await _userRepo.GetUserByUsernameAndPassword(username, password);

            return user;
        }

        public async Task<string> GetUserRoleByUsernameAsync(string username)
        {
            var role = await _userRepo.GetUserRoleByUsernameAsync(username);

            return role;
        }
    }
}