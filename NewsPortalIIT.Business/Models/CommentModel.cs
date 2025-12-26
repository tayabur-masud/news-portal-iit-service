namespace NewsPortalIIT.Business.Models;

/// <summary>
/// Represents a comment on a news article in the business layer.
/// Used for data transfer between the business layer and API layer.
/// Includes related author and news data.
/// </summary>
public class CommentModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the comment.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the text content of the comment.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the author who created this comment.
    /// </summary>
    public string AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the news article this comment belongs to.
    /// </summary>
    public string NewsId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the comment was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the author information for this comment.
    /// This is a navigation property populated by the service layer.
    /// </summary>
    public UserModel Author { get; set; }

    /// <summary>
    /// Gets or sets the news article this comment belongs to.
    /// This is a navigation property populated by the service layer.
    /// </summary>
    public NewsModel News { get; set; }
}
