using System.Linq.Expressions;

namespace Scantry.Data.Repositories
{
    public interface IRepository
    {
        IQueryable<T> All<T>() where T : class;
        Task AddAsync<T>(T entity) where T : class;
        Task<int> SaveChangesAsync();
        IQueryable<T?> FindAllAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
    }
}