using Microsoft.AspNetCore.Identity;
using Repository.Entities.Identity;
using System.Linq.Expressions;

namespace Repository.Interfaces
{
    public interface IUserRepository : IUserStore<UserEntity>
    {
        Task<IdentityResult> CreateAsync(UserEntity userEntity);
        Task<IdentityResult> UpdateAsync(UserEntity userEntity);
        Task<UserEntity?> GetSingleAsync(Expression<Func<UserEntity, bool>>? predicate = null, params Expression<Func<UserEntity, object>>[] includeProperties);
        Task<string> GetFullnameAsyncs(int userId);
        IQueryable<UserEntity> GetAllWithCondition(Expression<Func<UserEntity, bool>> predicate = null, params Expression<Func<UserEntity, object>>[] includeProperties);
    }
}