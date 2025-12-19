using Mapster;
using Microsoft.Extensions.DependencyInjection;
using NewsPortalIIT.Business.Mappings;
using NewsPortalIIT.Business.Services;

namespace NewsPortalIIT.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
    {
        services.AddMappingConfigurations();
        services.AddServices();

        return services;
    }

    public static IServiceCollection AddMappingConfigurations(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        new UserMapper().Register(config);
        new CommentMapper().Register(config);
        new NewsMapper().Register(config);

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<INewsService, NewsService>();
        services.AddScoped<ICommentService, CommentService>();

        return services;
    }
}
