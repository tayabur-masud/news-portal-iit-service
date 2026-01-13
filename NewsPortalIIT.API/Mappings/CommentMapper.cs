using Mapster;
using NewsPortalIIT.API.Models;
using NewsPortalIIT.Business.Models;

namespace NewsPortalIIT.API.Mappings;

internal class CommentMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CommentRequest, CommentModel>()
            .Map(dest => dest.Text, src => src.Text)
            .Map(dest => dest.NewsId, src => src.NewsId);

        config.NewConfig<CommentModel, CommentResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Text, src => src.Text)
            .Map(dest => dest.AuthorId, src => src.AuthorId)
            .Map(dest => dest.AuthorName, src => src.Author != null ? src.Author.Name : "Unknown")
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);
    }
}
