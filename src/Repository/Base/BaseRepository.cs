using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Repository.Entities.Base;
namespace Repository.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        public  IQueryable<T?> GetAll()
        {
            var context = new AppDbContext();
            var dbSet = context.Set<T>();
            return dbSet.AsQueryable().AsNoTracking();
        }

        public  IQueryable<T> GetAllWithCondition(Expression<Func<T, bool>> predicate = null,
            params Expression<Func<T, object>>[] includeProperties)
        {
            var context = new AppDbContext();
            var dbSet = context.Set<T>();
            IQueryable<T> queryable = dbSet.AsNoTracking();
            includeProperties = includeProperties?.Distinct().ToArray();
            if (includeProperties?.Any() ?? false)
            {
                Expression<Func<T, object>>[] array = includeProperties;
                foreach (Expression<Func<T, object>> navigationPropertyPath in array)
                {
                    queryable = queryable.Include(navigationPropertyPath);
                }
            }

            return predicate == null ? queryable : queryable.Where(predicate);
        }

        public async Task<IList<T?>> GetAllAsync()
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            return await dbSet.AsQueryable().AsNoTracking().ToListAsync();
        }

        public  IQueryable<T> Get(Expression<Func<T, bool>> predicate = null
            , bool isIncludeDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            IQueryable<T> source = dbSet.AsNoTracking();
            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            includeProperties = includeProperties?.Distinct().ToArray();
            if (includeProperties?.Any() ?? false)
            {
                Expression<Func<T, object>>[] array = includeProperties;
                foreach (Expression<Func<T, object>> navigationPropertyPath in array)
                {
                    source = source.Include(navigationPropertyPath);
                }
            }

            return isIncludeDeleted ? source.IgnoreQueryFilters() : source.Where((x) => x.DeletedTime == null);
        }


        public  T? GetById(int id)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            return dbSet.Find(id);
        }

        public  async Task<T?> GetByIdAsync(int id)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            return await dbSet.FindAsync(id);
        }

        /*public  async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, 
            bool isIncludeDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        {
            return await Get(predicate, isIncludeDeleted, includeProperties)
                .OrderByDescending(p => p.CreatedTime).FirstOrDefaultAsync();
        }*/

        public  async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate,
            bool isIncludeDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        {
            await using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            var query = dbSet.AsQueryable();

            if (!isIncludeDeleted)
            {
                query = query.Where(e => e.DeletedTime == null);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.FirstOrDefaultAsync(predicate);
        }

        public T Add(T entity)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            dbSet.Add(entity);
            context.SaveChanges();
            context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public  async Task<T> AddAsync(T entity)
        {
            await using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public  void AddRange(IEnumerable<T> entities)
        {
            using var context = new AppDbContext();
            context.AddRange(entities);
            context.SaveChanges();
        }

        public  async Task AddRangeAsync(IEnumerable<T?> entities)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            await dbSet.AddRangeAsync(entities);
            await context.SaveChangesAsync();
            foreach (var entity in entities)
            {
                context.Entry(entity).State = EntityState.Detached;
            }
        }

        public  void Update(T entity)
        {
            using var context = new AppDbContext();
            var tracker = context.Attach(entity);
            tracker.State = EntityState.Modified;
            context.SaveChanges();
            context.Entry(entity).State = EntityState.Detached;
        }

        public  async Task UpdateAsync(T entity)
        {
            using var context = new AppDbContext();
            var tracker = context.Attach(entity);
            tracker.State = EntityState.Modified;
            await context.SaveChangesAsync();
            context.Entry(entity).State = EntityState.Detached;
        }

        public  async Task UpdateRangeAsync(IEnumerable<T?> entities)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            dbSet.UpdateRange(entities);
            await context.SaveChangesAsync();
            foreach (var entity in entities)
            {
                context.Entry(entity).State = EntityState.Detached;
            }
        }

        public  void Delete(T? entity)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public  async Task DeleteAsync(T? entity)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }

        public  void RemoveRange(IEnumerable<T?> entities)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            dbSet.RemoveRange(entities);
            context.SaveChanges();
        }

        public  async Task RemoveRangeAsync(IEnumerable<T?> entities)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            dbSet.RemoveRange(entities);
            await context.SaveChangesAsync();
        }

        public  IQueryable<T?> FindByCondition(Expression<Func<T?, bool>> expression)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            return dbSet.Where(expression).AsQueryable().AsNoTracking();
        }

        public  async Task<IList<T?>> FindByConditionAsync(Expression<Func<T?, bool>> expression)
        {
            await using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            return await dbSet.Where(expression).AsQueryable().AsNoTracking().ToListAsync();
        }

        public  async Task<T?> GetSingleAsync(Expression<Func<T, bool>>? predicate = null,
            params Expression<Func<T, object>>[] includeProperties)
            => await Get(predicate, includeProperties).FirstOrDefaultAsync();

        public  IQueryable<T> Get(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            IQueryable<T> reault = dbSet.AsNoTracking();
            if (predicate != null)
            {
                reault = reault.Where(predicate);
            }

            includeProperties = includeProperties?.Distinct().ToArray();
            if (includeProperties?.Any() ?? false)
            {
                Expression<Func<T, object>>[] array = includeProperties;
                foreach (Expression<Func<T, object>> navigationPropertyPath in array)
                {
                    reault = reault.Include(navigationPropertyPath);
                }
            }

            return reault.Where(x => x.DeletedTime == null);
        }

        public  void TryAttach(T entity)
        {
            try
            {
                using var context = new AppDbContext();
                var dbSet = context.Set<T>();
                if (context.Entry(entity).State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                }
            }
            catch
            {
            }
        }

        protected void TryAttachRange(ICollection<T> entities)
        {
            try
            {
                using var context = new AppDbContext();
                var dbSet = context.Set<T>();
                foreach (var entity in entities)
                {
                    if (context.Entry(entity).State != EntityState.Detached)
                    {
                        entities.Remove(entity);
                    }
                }
                dbSet.AttachRange(entities);
            }
            catch
            {
            }
        }

        public  async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            return await dbSet.FirstOrDefaultAsync(predicate);
        }
    }
}
