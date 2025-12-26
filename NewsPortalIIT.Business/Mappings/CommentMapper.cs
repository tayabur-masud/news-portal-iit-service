using Mapster;
using MongoDB.Bson;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;

namespace NewsPortalIIT.Business.Mappings;

/// <summary>
/// Configures Mapster type adapter mappings between <see cref="Comment"/> domain entity and <see cref="CommentModel"/> business model.
/// Handles bidirectional conversion including ObjectId to string transformations for IDs and foreign keys.
/// </summary>
internal class CommentMapper : IRegister
{
    /// <summary>
    /// Registers the mapping configurations for Comment entity and CommentModel.
    /// </summary>
    /// <param name="config">The Mapster type adapter configuration to register mappings with.</param>
    /// <remarks>
    /// Configures two-way mappings:
    /// <list type="bullet">
    /// <item><description>CommentModel to Comment: Converts string IDs to ObjectId, generates new ID if empty, converts foreign keys</description></item>
    /// <item><description>Comment to CommentModel: Converts ObjectId to string for ID and all foreign keys</description></item>
    /// </list>
    /// </remarks>
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
