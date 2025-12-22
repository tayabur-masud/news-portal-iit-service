using Microsoft.EntityFrameworkCore.Storage;
using NewsPortalIIT.Domain.UnitOfWork;

namespace NewsPortalIIT.Persistence.UnitOfWork;

/// <summary>
/// Implementation of the Unit of Work pattern using Entity Framework Core transactions.
/// </summary>
/// <param name="context">The application database context.</param>
public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    /// <inheritdoc/>
    public async Task BeginTransactionAsync()
    {
        _transaction = await context.Database.BeginTransactionAsync();
    }

    /// <inheritdoc/>
    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <inheritdoc/>
    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _transaction?.Dispose();
        context.Dispose();
    }
}
