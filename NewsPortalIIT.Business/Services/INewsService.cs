using NewsPortalIIT.Business.Models;

namespace NewsPortalIIT.Business.Services;

/// <summary>
/// Defines the contract for news article management operations.
/// </summary>
public interface INewsService
{
    /// <summary>
    /// Retrieves a specific news article by its unique identifier, including related author and comments.
    /// </summary>
    /// <param name="id">The unique identifier of the news article.</param>
    /// <returns>The news model with populated author and comments.</returns>
    /// <exception cref="Exception">Thrown when the news article is not found.</exception>
    Task<NewsModel> GetByIdAsync(string id);

    /// <summary>
    /// Creates a new news article in the system.
    /// </summary>
    /// <param name="user">The news model containing the article information to create.</param>
    Task CreateAsync(NewsModel user);

    /// <summary>
    /// Updates an existing news article's information.
    /// </summary>
    /// <param name="user">The news model containing the updated article information.</param>
    /// <exception cref="Exception">Thrown when the news article is not found.</exception>
    Task UpdateAsync(NewsModel user);

    /// <summary>
    /// Deletes a news article and all its associated comments from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the news article to delete.</param>
    Task DeleteAsync(string id);

    /// <summary>
    /// Retrieves a paginated list of news articles with optional search filtering.
    /// Includes related author and comments data for each article.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="searchTerm">Optional search term to filter news by title.</param>
    /// <returns>A paged result containing news models with populated relationships.</returns>
    Task<PagedResult<NewsModel>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
}
