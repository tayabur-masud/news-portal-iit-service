using Mapster;
using MongoDB.Bson;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;

namespace NewsPortalIIT.Business.Mappings;

/// <summary>
/// Configures Mapster type adapter mappings between <see cref="News"/> domain entity and <see cref="NewsModel"/> business model.
/// Handles bidirectional conversion including ObjectId transformations and navigation properties.
/// </summary>
internal class NewsMapper : IRegister
{
    /// <summary>
    /// Registers the mapping configurations for News entity and NewsModel.
    /// </summary>
    /// <param name="config">The Mapster type adapter configuration to register mappings with.</param>
    /// <remarks>
    /// Configures two-way mappings:
    /// <list type="bullet">
    /// <item><description>NewsModel to News: Converts string IDs to ObjectId, generates new ID if empty, ignores navigation properties</description></item>
    /// <item><description>News to NewsModel: Converts ObjectId to string, includes Author and Comments navigation properties</description></item>
    /// </list>
    /// </remarks>
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
            .Map(dest => dest.Author, src => src.Author)
            .Map(dest => dest.Comments, src => src.Comments)
            .IgnoreNonMapped(true);
    }
}
