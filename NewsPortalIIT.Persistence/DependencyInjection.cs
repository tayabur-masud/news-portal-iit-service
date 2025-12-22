using Microsoft.Extensions.DependencyInjection;
using NewsPortalIIT.Domain.Repositories;
using NewsPortalIIT.Domain.UnitOfWork;
using NewsPortalIIT.Persistence.Repositories;

namespace NewsPortalIIT.Persistence;

/// <summary>
/// Provides extension methods for registering persistence layer services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the persistence layer services, including repositories and the unit of work, to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }

    /// <summary>
    /// Adds the generic repositories to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the repositories to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}
