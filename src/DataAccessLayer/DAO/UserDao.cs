using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.DTO.User;
using BusinessObject.Entities;
using DataAccessLayer.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utility.Enum;

namespace DataAccessLayer.DAO
{
    public class UserDao : BaseDAO<User>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signinManager;

        public UserDao(UserManager<User> userManager, SignInManager<User> signinManager)
        {
            _userManager = userManager;
            _signinManager = signinManager;
        }
        public async Task<IdentityResult> CreateCustomerAsync(User user, string password)
        {
            IdentityResult? result = null;

            result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user, UserRole.Customer.ToString());
            }

            return result;
        }

        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
            {
                return null;
            }

            var result = await _signinManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded) return null;

            return user;
        }

        public async Task<string> GetUserRoleByUsernameAsync(string username)
        {
            // Find the user by username
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return null;
            }

            // Get the role for the user
            var roles = await _userManager.GetRolesAsync(user);

            // Since each user has only one role, return the first role
            return roles.FirstOrDefault();
        }
    }
}