using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BusinessObject.Entities.Identity;
using DataAccessLayer.DAO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Repository.Base;
using Repository.Interface;
using Utility.Enum;

namespace Repository.Repositories
{
    public class UserRepository : UserStore<UserEntity, RoleEntity, AppDbContext, int>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IdentityResult> CreateAsync(UserEntity userEntity) => await UserDao.CreateAsync(userEntity);

        public async Task<UserEntity> GetUserByUsernameAndPassword(string username, string password) => await UserDao.GetUserByUsernameAndPassword(username, password);

        public async Task<string> GetUserRoleByUsernameAsync(string username) => await UserDao.GetUserRoleByUsernameAsync(username);
        public async Task<UserEntity> GetUserByEmail(string email) => await UserDao.GetUserByEmailAsync(email);
        public async Task<UserEntity> GetUserByUserName(string userName) => await UserDao.GetUserByUserNameAsync(userName);

        public async Task AddUserToRoleAsync(UserEntity user, int roleId) => await UserDao.AddUserToRoleAsync(user, roleId);
    }
}