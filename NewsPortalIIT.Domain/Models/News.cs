using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NewsPortalIIT.Domain.Models;

/// <summary>
/// Represents a news article in the system.
/// </summary>
public class News
{
    /// <summary>
    /// Gets or sets the unique identifier for the news article.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the news article.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the body content of the news article.
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who authored the news article.
    /// </summary>
    public ObjectId AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the news article was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the author of the news article.
    /// </summary>
    public User Author { get; set; }

    /// <summary>
    /// Gets or sets the collection of comments associated with the news article.
    /// </summary>
    public ICollection<Comment> Comments { get; set; }
}
