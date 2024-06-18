using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BusinessObject.Entities.Base;
using System.Collections.Generic;

namespace DataAccessLayer.Base
{
    public class BaseDao<T> where T : BaseEntity, new()
    {
        private static readonly AppDbContext _context = new();
        private static DbSet<T> _dbSet = _context.Set<T>();

        public static IQueryable<T?> GetAll()
        {
            return _dbSet.AsQueryable().AsNoTracking();
        }

        public static IQueryable<T> GetAllWithCondition(Expression<Func<T, bool>> predicate = null, 
            params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = _dbSet.AsNoTracking();
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

        public static async Task<List<T?>> GetAllAsync()
        {
            return await _dbSet.AsQueryable().AsNoTracking().ToListAsync();
        }

        public static IQueryable<T> Get(Expression<Func<T, bool>> predicate = null
            , bool isIncludeDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> source = _dbSet.AsNoTracking();
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


        public static T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public static async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public static async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, 
            bool isIncludeDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        {
            return await Get(predicate, isIncludeDeleted, includeProperties)
                .OrderByDescending(p => p.CreatedTime).FirstOrDefaultAsync();
        }
        public async Task<T> GetSingleAsyncWithProperties(Expression<Func<T, bool>> predicate,
            bool isIncludeDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbSet.AsQueryable();

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

        public static T Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
            _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public static async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public static void AddRange(IEnumerable<T> entities)
        {
            _context.AddRange(entities);
            _context.SaveChanges();
        }

        public static async Task AddRangeAsync(IEnumerable<T?> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
        }

        public static void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
            _context.Entry(entity).State = EntityState.Detached;
        }

        public static async Task UpdateAsync(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _context.Entry(entity).State = EntityState.Detached;
        }

        public static async Task UpdateRangeAsync(IEnumerable<T?> entities)
        {
            _dbSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
        }

        public static void Delete(T? entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public static async Task DeleteAsync(T? entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public static void RemoveRange(IEnumerable<T?> entities)
        {
            _dbSet.RemoveRange(entities);
            _context.SaveChanges();
        }

        public static async Task RemoveRangeAsync(IEnumerable<T?> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public static IQueryable<T?> FindByCondition(Expression<Func<T?, bool>> expression)
        {
            return _dbSet.Where(expression).AsQueryable().AsNoTracking();
        }

        public static async Task<IList<T?>> FindByConditionAsync(Expression<Func<T?, bool>> expression)
        {
            return await _dbSet.Where(expression).AsQueryable().AsNoTracking().ToListAsync();
        }

        public static async Task<T?> GetSingleAsync(Expression<Func<T, bool>>? predicate = null,
            params Expression<Func<T, object>>[] includeProperties)
            => await Get(predicate, includeProperties).FirstOrDefaultAsync();

        public static IQueryable<T> Get(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> reault = _dbSet.AsNoTracking();
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

        public static void TryAttach(T entity)
        {
            try
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
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
                foreach (var entity in entities)
                {
                    if (_context.Entry(entity).State != EntityState.Detached)
                    {
                        entities.Remove(entity);
                    }
                }
                _dbSet.AttachRange(entities);
            }
            catch
            {
            }
        }
    }
}