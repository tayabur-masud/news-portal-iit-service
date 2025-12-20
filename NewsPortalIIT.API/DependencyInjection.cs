using Mapster;
using NewsPortalIIT.API.Mappings;

namespace NewsPortalIIT.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiLayer(this IServiceCollection services)
    {
        services.AddMappingConfigurations();

        return services;
    }

    public static IServiceCollection AddMappingConfigurations(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        new UserMapper().Register(config);
        new NewsMapper().Register(config);

        return services;
    }
}
