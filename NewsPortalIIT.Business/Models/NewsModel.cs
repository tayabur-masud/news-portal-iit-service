namespace NewsPortalIIT.Business.Models;

/// <summary>
/// Represents a news article in the business layer.
/// Used for data transfer between the business layer and API layer.
/// Includes related author and comments data.
/// </summary>
public class NewsModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the news article.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the news article.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the body content of the news article.
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the author who created this news article.
    /// </summary>
    public string AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the news article was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the author information for this news article.
    /// This is a navigation property populated by the service layer.
    /// </summary>
    public UserModel Author { get; set; }

    /// <summary>
    /// Gets or sets the collection of comments associated with this news article.
    /// This is a navigation property populated by the service layer.
    /// </summary>
    public ICollection<CommentModel> Comments { get; set; }
}
