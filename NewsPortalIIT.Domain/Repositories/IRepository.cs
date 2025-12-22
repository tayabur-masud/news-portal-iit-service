using System.Linq.Expressions;
using MongoDB.Bson;

namespace NewsPortalIIT.Domain.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetByIdAsync(ObjectId id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(ObjectId id);
    Task DeleteAsync(Expression<Func<T, bool>> predicate);
}
