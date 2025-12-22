namespace NewsPortalIIT.Domain.UnitOfWork;

/// <summary>
/// Defines the interface for a Unit of Work pattern to manage transactions across multiple repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// asynchronously starts a new transaction.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task BeginTransactionAsync();

    /// <summary>
    /// asynchronously commits the current transaction.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CommitAsync();

    /// <summary>
    /// asynchronously rolls back the current transaction in case of an error.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RollbackAsync();
}
