using System.Linq.Expressions;

namespace UserManager.Common.Interfaces.Repos
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task CreateAsync(T entity);
        Task<T> CreateOrUpdateAsync(T entity, Expression<Func<T, bool>> predicate);
    }
}
