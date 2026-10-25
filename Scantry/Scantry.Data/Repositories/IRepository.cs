using System.Linq.Expressions;

namespace Scantry.Data.Repositories
{
    public interface IRepository
    {
        IQueryable<T> All<T>() where T : class;
        IQueryable<T> AllTracked<T>() where T : class;
        IQueryable<T> GetAllWithPredicate<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task AddAsync<T>(T entity) where T : class;
        Task<int> DeleteAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task<int> SaveChangesAsync();
        Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task<T?> FirstOrDefaultTrackedAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
    }
}