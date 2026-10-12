using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Scantry.Data.Repositories
{
    public class Repository(ApplicationDbContext context) : IRepository
    {
        private readonly ApplicationDbContext context = context;

        public IQueryable<T> All<T>() where T : class
        {
            return context.Set<T>();
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            await context.Set<T>().AddAsync(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public IQueryable<T?> FindAllAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return context.Set<T>().Where(predicate).AsNoTracking();
        }
    }
}