using Microsoft.Extensions.DependencyInjection;
using NewsPortalIIT.Domain.Repositories;
using NewsPortalIIT.Domain.UnitOfWork;
using NewsPortalIIT.Persistence.Repositories;

namespace NewsPortalIIT.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}
