using NewsPortalIIT.Business.Models;

namespace NewsPortalIIT.Business.Services;

/// <summary>
/// Defines the contract for comment management operations.
/// </summary>
public interface ICommentService
{
    /// <summary>
    /// Retrieves all comments associated with a specific news article, including author information.
    /// </summary>
    /// <param name="id">The unique identifier of the news article.</param>
    /// <returns>A collection of comment models with populated author data.</returns>
    Task<IEnumerable<CommentModel>> GetByNewsIdAsync(string id);

    /// <summary>
    /// Creates a new comment in the system.
    /// </summary>
    /// <param name="comment">The comment model containing the comment information to create.</param>
    Task CreateAsync(CommentModel comment);

    /// <summary>
    /// Updates an existing comment's information.
    /// </summary>
    /// <param name="comment">The comment model containing the updated comment information.</param>
    /// <exception cref="Exception">Thrown when the comment is not found.</exception>
    Task UpdateAsync(CommentModel comment);

    /// <summary>
    /// Deletes a comment from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the comment to delete.</param>
    Task DeleteAsync(string id);
}
