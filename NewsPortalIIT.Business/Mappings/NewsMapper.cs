using Mapster;
using MongoDB.Bson;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;

namespace NewsPortalIIT.Business.Mappings;

internal class NewsMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<NewsModel, News>()
            .Map(dest => dest.Id, src => string.IsNullOrEmpty(src.Id) ? ObjectId.GenerateNewId() : ObjectId.Parse(src.Id))
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Body, src => src.Body)
            .Map(dest => dest.AuthorId, src => ObjectId.Parse(src.AuthorId))
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .IgnoreNonMapped(true);

        config.NewConfig<News, NewsModel>()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Body, src => src.Body)
            .Map(dest => dest.AuthorId, src => src.AuthorId.ToString())
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.Author, src => src.Author);
    }
}
