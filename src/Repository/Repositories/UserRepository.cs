using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Entities.Identity;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class UserRepository : UserStore<UserEntity, RoleEntity, AppDbContext, int>, IUserRepository
    {
        private readonly AppDbContext _context;
        private  UserManager<UserEntity> _userManager;
        private  SignInManager<UserEntity> _signinManager;

        public UserRepository(AppDbContext context, UserManager<UserEntity> 
            userManager, SignInManager<UserEntity> signInManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _signinManager = signInManager;
        }

        public Task<string> GetFullnameAsyncs(int userId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserEntity> GetAllWithCondition(Expression<Func<UserEntity, bool>> predicate = null,
            params Expression<Func<UserEntity, object>>[] includeProperties)
        {
            var context = new AppDbContext();
            var dbSet = context.Set<UserEntity>();
            IQueryable<UserEntity> queryable = dbSet.AsNoTracking();
            includeProperties = includeProperties?.Distinct().ToArray();
            if (includeProperties?.Any() ?? false)
            {
                Expression<Func<UserEntity, object>>[] array = includeProperties;
                foreach (Expression<Func<UserEntity, object>> navigationPropertyPath in array)
                {
                    queryable = queryable.Include(navigationPropertyPath);
                }
            }

            return predicate == null ? queryable : queryable.Where(predicate);
        }

        public  async Task<IdentityResult> CreateAsync(UserEntity user)
        {
            await using var context = new AppDbContext();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public  async Task<IdentityResult> UpdateAsync(UserEntity user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public  async Task<UserEntity> GetUserByUserNameAsync(string username)
        {

            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public  async Task<UserEntity> GetUserByEmailAsync(string email)
        {

            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public  async Task<UserEntity?> GetSingleAsync(Expression<Func<UserEntity, bool>>? predicate = null,
            params Expression<Func<UserEntity, object>>[] includeProperties)
        => await Get(predicate, includeProperties).FirstOrDefaultAsync();

        public  IQueryable<UserEntity> Get(Expression<Func<UserEntity, bool>>? predicate = null, params Expression<Func<UserEntity, object>>[] includeProperties)
        {
            IQueryable<UserEntity> reault = _context.Users.AsNoTracking();
            if (predicate != null)
            {
                reault = reault.Where(predicate);
            }

            includeProperties = includeProperties?.Distinct().ToArray();
            if (includeProperties?.Any() ?? false)
            {
                Expression<Func<UserEntity, object>>[] array = includeProperties;
                foreach (Expression<Func<UserEntity, object>> navigationPropertyPath in array)
                {
                    reault = reault.Include(navigationPropertyPath);
                }
            }

            return reault.Where(x => x.DeletedTime == null);
        }
    }
}