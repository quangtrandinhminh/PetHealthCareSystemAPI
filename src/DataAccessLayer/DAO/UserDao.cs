using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BusinessObject.DTO.User;
using BusinessObject.Entities.Identity;
using DataAccessLayer.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Utility.Enum;

namespace DataAccessLayer.DAO
{
    public class UserDao
    {
        private static AppDbContext _context = new();
        private static UserManager<UserEntity> _userManager;
        private static SignInManager<UserEntity> _signinManager;


        public static async Task<IdentityResult> CreateAsync(UserEntity user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public static async Task<UserEntity> GetUserByUserNameAsync(string username)
        {

            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public static async Task<UserEntity> GetUserByEmailAsync(string email)
        {

            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<UserEntity> GetUserByUsernameAndPassword(string username, string password)
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

        public static async Task<string> GetUserRoleByUsernameAsync(string username)
        {
            // Find the userEntity by username
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
            {
                return null;
            }

            var role = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (role == null)
            {
                return null;
            }

            var roleName = await _context.Roles.FirstOrDefaultAsync(x => x.Id == role.RoleId);
            return roleName.Name;
        }

        public static async Task AddUserToRoleAsync(UserEntity user, int roleId)
        {
            await _context.UserRoles.AddAsync(new IdentityUserRole<int>
            {
                RoleId = roleId,
                UserId = user.Id
            });
            await _context.SaveChangesAsync();
        }
    }
}