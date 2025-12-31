using Mapster;
using NewsPortalIIT.API.Models;
using NewsPortalIIT.Business.Models;

namespace NewsPortalIIT.API.Mappings;

internal class UserMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserRequest, UserModel>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email);

        config.NewConfig<UserModel, UserResponse>()
            .Map(dest => dest.Id, src => string.IsNullOrEmpty(src.Id) ? null : src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email);
    }
}
