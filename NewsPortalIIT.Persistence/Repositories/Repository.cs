using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using NewsPortalIIT.Domain.Repositories;
using System.Linq.Expressions;

namespace NewsPortalIIT.Persistence.Repositories;

/// <summary>
/// Implementation of the generic repository using Entity Framework Core.
/// </summary>
/// <typeparam name="T">The type of the entity. Must be a class.</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{T}"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<T?> GetByIdAsync(ObjectId id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <inheritdoc/>
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(ObjectId id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        var entities = await _dbSet.Where(predicate).ToListAsync();
        if (entities.Count != 0)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
    {
        var query = _dbSet.Where(predicate);
        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items, totalCount);
    }
}
