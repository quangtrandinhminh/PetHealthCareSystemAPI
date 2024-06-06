using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Repository;

namespace DataAccessLayer.Base
{
    public class BaseDAO<T> where T : class, new()
    {
        protected static AppDbContext _context = new AppDbContext();
        protected static DbSet<T> _dbSet = _context.Set<T>();

        public static void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public static IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public static void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public static void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public static IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).AsNoTracking();
        }

        public static AppDbContext GetDbContext()
        {
            return _context;
        }
        
        public static void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            _context.SaveChanges();
        }
        
        public static void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            _context.SaveChanges();
        }
    }
}