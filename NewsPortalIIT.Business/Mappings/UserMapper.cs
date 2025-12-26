using Mapster;
using MongoDB.Bson;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;

namespace NewsPortalIIT.Business.Mappings;

/// <summary>
/// Configures Mapster type adapter mappings between <see cref="User"/> domain entity and <see cref="UserModel"/> business model.
/// Handles bidirectional conversion including ObjectId to string transformations.
/// </summary>
internal class UserMapper : IRegister
{
    /// <summary>
    /// Registers the mapping configurations for User entity and UserModel.
    /// </summary>
    /// <param name="config">The Mapster type adapter configuration to register mappings with.</param>
    /// <remarks>
    /// Configures two-way mappings:
    /// <list type="bullet">
    /// <item><description>UserModel to User: Converts string IDs to ObjectId, generates new ID if empty</description></item>
    /// <item><description>User to UserModel: Converts ObjectId to string representation</description></item>
    /// </list>
    /// </remarks>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserModel, User>()
            .Map(dest => dest.Id, src => string.IsNullOrEmpty(src.Id) ? ObjectId.GenerateNewId() : ObjectId.Parse(src.Id))
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email)
            .IgnoreNonMapped(true);

        config.NewConfig<User, UserModel>()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email);
    }
}
