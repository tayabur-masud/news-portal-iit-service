using Mapster;
using Microsoft.Extensions.DependencyInjection;
using NewsPortalIIT.Business.Mappings;
using NewsPortalIIT.Business.Services;

namespace NewsPortalIIT.Business;

/// <summary>
/// Provides extension methods for configuring business layer services and dependencies.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers all business layer services and configurations with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
    {
        services.AddMappingConfigurations();
        services.AddServices();

        return services;
    }

    /// <summary>
    /// Configures Mapster type adapters for domain model to business model mappings.
    /// </summary>
    /// <param name="services">The service collection to add configurations to.</param>
    /// <returns>The service collection for method chaining.</returns>
    private static IServiceCollection AddMappingConfigurations(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        new UserMapper().Register(config);
        new CommentMapper().Register(config);
        new NewsMapper().Register(config);

        return services;
    }

    /// <summary>
    /// Registers all business layer service implementations with scoped lifetime.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<INewsService, NewsService>();
        services.AddScoped<ICommentService, CommentService>();

        return services;
    }
}
