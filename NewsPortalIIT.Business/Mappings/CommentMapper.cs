using Mapster;
using MongoDB.Bson;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;

namespace NewsPortalIIT.Business.Mappings;

internal class CommentMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CommentModel, Comment>()
            .Map(dest => dest.Id, src => string.IsNullOrEmpty(src.Id) ? ObjectId.GenerateNewId() : ObjectId.Parse(src.Id))
            .Map(dest => dest.Text, src => src.Text)
            .Map(dest => dest.AuthorId, src => ObjectId.Parse(src.AuthorId))
            .Map(dest => dest.NewsId, src => ObjectId.Parse(src.NewsId))
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .IgnoreNonMapped(true);

        config.NewConfig<Comment, CommentModel>()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.Text, src => src.Text)
            .Map(dest => dest.AuthorId, src => src.AuthorId.ToString())
            .Map(dest => dest.NewsId, src => src.NewsId.ToString())
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .IgnoreNonMapped(true);
    }
}
