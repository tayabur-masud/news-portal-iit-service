using Mapster;
using NewsPortalIIT.API.Models;
using NewsPortalIIT.Business.Models;

namespace NewsPortalIIT.API.Mappings;

public class NewsMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<NewsRequest, NewsModel>()
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Body, src => src.Body)
            .Map(dest => dest.AuthorId, src => src.AuthorId);

        config.NewConfig<NewsModel, NewsResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Body, src => src.Body)
            .Map(dest => dest.AuthorId, src => src.AuthorId)
            .Map(dest => dest.AuthorName, src => src.Author != null ? src.Author.Name : "Unknown")
            .Map(dest => dest.NoOfComments, src => src.Comments != null ? src.Comments.Count : 0)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.Comments, src => src.Comments);
    }
}
