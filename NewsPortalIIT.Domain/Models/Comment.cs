using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NewsPortalIIT.Domain.Models;

/// <summary>
/// Represents a comment on a news article.
/// </summary>
public class Comment
{
    /// <summary>
    /// Gets or sets the unique identifier for the comment.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// Gets or sets the text content of the comment.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who authored the comment.
    /// </summary>
    public ObjectId AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the news article the comment belongs to.
    /// </summary>
    public ObjectId NewsId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the comment was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the author of the comment.
    /// </summary>
    public User Author { get; set; }

    /// <summary>
    /// Gets or sets the news article the comment belongs to.
    /// </summary>
    public News News { get; set; }
}
