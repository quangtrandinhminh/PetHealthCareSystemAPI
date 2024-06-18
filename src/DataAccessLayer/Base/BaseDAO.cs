using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BusinessObject.Entities.Base;

namespace DataAccessLayer.Base
{
    public class BaseDao<T> where T : BaseEntity, new()
    {
        public static IQueryable<T?> GetAll()
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            return dbSet.AsQueryable().AsNoTracking();
        }

        public static async Task<List<T?>> GetAllAsync()
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            return await dbSet.AsQueryable().AsNoTracking().ToListAsync();
        }

        public static T? GetById(int id)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            return dbSet.Find(id);
        }

        public static async Task<T?> GetByIdAsync(int id)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            return await dbSet.FindAsync(id);
        }

        public static T Add(T entity)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            dbSet.Add(entity);
            context.SaveChanges();
            context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public static async Task<T> AddAsync(T entity)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public static void AddRange(IEnumerable<T> entities)
        {
            using var context = new AppDbContext();
            context.AddRange(entities);
            context.SaveChanges();
        }

        public static async Task AddRangeAsync(IEnumerable<T?> entities)
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

        public static void Update(T entity)
        {
            using var context = new AppDbContext();
            var tracker = context.Attach(entity);
            tracker.State = EntityState.Modified;
            context.SaveChanges();
            context.Entry(entity).State = EntityState.Detached;
        }

        public static async Task UpdateAsync(T entity)
        {
            using var context = new AppDbContext();
            var tracker = context.Attach(entity);
            tracker.State = EntityState.Modified;
            await context.SaveChangesAsync();
            context.Entry(entity).State = EntityState.Detached;
        }

        public static async Task UpdateRangeAsync(IEnumerable<T?> entities)
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

        public static void Delete(T? entity)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public static async Task DeleteAsync(T? entity)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static void RemoveRange(IEnumerable<T?> entities)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            dbSet.RemoveRange(entities);
            context.SaveChanges();
        }

        public static async Task RemoveRangeAsync(IEnumerable<T?> entities)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            dbSet.RemoveRange(entities);
            await context.SaveChangesAsync();
        }

        public static IQueryable<T?> FindByCondition(Expression<Func<T?, bool>> expression)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            return dbSet.Where(expression).AsQueryable().AsNoTracking();
        }

        public static async Task<IList<T?>> FindByConditionAsync(Expression<Func<T?, bool>> expression)
        {
            using var context = new AppDbContext();
            var dbSet = context.Set<T>();
            return await dbSet.Where(expression).AsQueryable().AsNoTracking().ToListAsync();
        }
    }
}
