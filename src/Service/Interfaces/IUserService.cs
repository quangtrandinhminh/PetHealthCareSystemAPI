using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Identity;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateCustomerAsync(User user, string password);
        Task<User> GetUserByUsernameAndPassword(string username, string password);
        Task<string> GetUserRoleByUsernameAsync(string username);
    }
}