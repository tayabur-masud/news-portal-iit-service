using System.Linq.Expressions;
using MongoDB.Bson;

namespace NewsPortalIIT.Domain.Repositories;

/// <summary>
/// Defines the generic repository interface for CRUD operations on entities of type T.
/// </summary>
/// <typeparam name="T">The type of the entity. Must be a class.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Retrieves all entities of type T from the repository.
    /// </summary>
    /// <returns>A collection of all entities.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Retrieves entities of type T that match the specified predicate.
    /// </summary>
    /// <param name="predicate">The expression to filter the entities.</param>
    /// <returns>A collection of matching entities.</returns>
    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Retrieves a single entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<T?> GetByIdAsync(ObjectId id);

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    Task AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity with updated values.</param>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    Task DeleteAsync(ObjectId id);

    /// <summary>
    /// Deletes entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">The expression to filter entities for deletion.</param>
    /// <inheritdoc/>
    Task DeleteAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Retrieves a paged collection of entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">The expression to filter the entities.</param>
    /// <param name="pageNumber">The page number (1-indexed).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A tuple containing the items for the current page and the total count of matching entities.</returns>
    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize);
}
