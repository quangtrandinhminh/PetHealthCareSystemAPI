using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Entities;
using DataAccessLayer;
using Microsoft.AspNetCore.Identity;
using Repository.Base;
using Repository.Interface;

namespace Repository.Class
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signinManager;
        private readonly UserDao _userDao;

        public UserRepository(UserManager<User> userManager, SignInManager<User> signinManager)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _userDao = new UserDao(_userManager, _signinManager);
        }

        public async Task<IdentityResult> CreateCustomerAsync(User user, string password)
        {
            var result = await _userDao.CreateCustomerAsync(user, password);

            return result;
        }

        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            var user = await _userDao.GetUserByUsernameAndPassword(username, password);

            return user;
        }

        public async Task<string> GetUserRoleByUsernameAsync(string username)
        {
            var role = await _userDao.GetUserRoleByUsernameAsync(username);

            return role;
        }
    }
}