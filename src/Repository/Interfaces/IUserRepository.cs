using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BusinessObject.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Repository.Interface
{
    public interface IUserRepository : IUserStore<UserEntity>
    {
        Task<IdentityResult> CreateAsync(UserEntity userEntity);
        Task<UserEntity> GetUserByUsernameAndPassword(string username, string password);
        Task<string> GetUserRoleByUsernameAsync(string username);

        Task<UserEntity> GetUserByEmail(string email);
        Task<UserEntity> GetUserByUserName(string userName);

        Task AddUserToRoleAsync(UserEntity user, int roleId);
    }
}