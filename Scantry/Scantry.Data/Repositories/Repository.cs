using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Scantry.Data.Repositories
{
    public class Repository(ApplicationDbContext context) : IRepository
    {
        public IQueryable<T> All<T>() where T : class
        {
            return context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> AllTracked<T>() where T : class
        {
            return context.Set<T>();
        }

        public IQueryable<T> GetAllWithPredicate<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return context.Set<T>().Where(predicate).AsNoTracking();
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            await context.Set<T>().AddAsync(entity);
        }

        public async Task<int> DeleteAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await context.Set<T>().Where(predicate).ExecuteDeleteAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public async Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> FirstOrDefaultTrackedAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await context.Set<T>().FirstOrDefaultAsync(predicate);
        }
    }
}