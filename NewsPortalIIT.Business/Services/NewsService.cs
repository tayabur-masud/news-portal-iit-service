using Mapster;
using MongoDB.Bson;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;
using NewsPortalIIT.Domain.Repositories;
using System.Linq.Expressions;

namespace NewsPortalIIT.Business.Services;

/// <summary>
/// Provides implementation for news article management operations.
/// Handles CRUD operations for news articles with optimized batch loading of related author and comment data.
/// Uses unit of work pattern for transactional operations.
/// </summary>
public class NewsService : INewsService
{
    private readonly IRepository<News> _newsRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Comment> _commentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="NewsService"/> class.
    /// </summary>
    /// <param name="newsRepository">The repository for news data access.</param>
    /// <param name="userRepository">The repository for user data access.</param>
    /// <param name="commentRepository">The repository for comment data access.</param>
    public NewsService(
        IRepository<News> newsRepository,
        IRepository<User> userRepository,
        IRepository<Comment> commentRepository)
    {
        _newsRepository = newsRepository;
        _userRepository = userRepository;
        _commentRepository = commentRepository;
    }

    /// <inheritdoc/>
    public async Task<NewsModel> GetByIdAsync(string id)
    {
        var news = await _newsRepository.GetByIdAsync(ObjectId.Parse(id));
        if (news is null)
        {
            throw new Exception("News not found");
        }

        var newsModel = news.Adapt<NewsModel>();
        await LoadRelatedData(newsModel);

        return newsModel;
    }

    /// <inheritdoc/>
    public async Task CreateAsync(NewsModel newsModel)
    {
        var news = newsModel.Adapt<News>();
        news.CreatedAt = DateTime.UtcNow;
        await _newsRepository.AddAsync(news);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(NewsModel newsModel)
    {
        var newsFromDb = await _newsRepository.GetByIdAsync(ObjectId.Parse(newsModel.Id));

        if (newsFromDb is null)
        {
            throw new Exception("News not found");
        }

        newsModel.Adapt(newsFromDb);

        await _newsRepository.UpdateAsync(newsFromDb);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This method deletes the news article and all its associated comments.
    /// Note: Transactions are disabled as the current MongoDB configuration does not support them.
    /// </remarks>
    public async Task DeleteAsync(string id)
    {
        try
        {
            await _commentRepository.DeleteAsync(c => c.NewsId == ObjectId.Parse(id));
            await _newsRepository.DeleteAsync(ObjectId.Parse(id));
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This method uses optimized batch loading to minimize database queries:
    /// 1. Retrieves the requested page of news articles
    /// 2. Batch loads all unique authors for the news articles
    /// 3. Batch loads all comments for the news articles
    /// 4. Batch loads all unique authors for the comments
    /// This approach significantly improves performance compared to loading related data individually.
    /// </remarks>
    public async Task<PagedResult<NewsModel>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        Expression<Func<News, bool>> predicate = n => true;
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            predicate = n => n.Title.ToLower().Contains(searchTerm.ToLower());
        }

        var (newsList, totalCount) = await _newsRepository.GetPagedAsync(predicate, pageNumber, pageSize);

        if (newsList.Any())
        {
            // Batch load Authors
            var authorIds = newsList
                .Where(n => n.AuthorId != ObjectId.Empty)
                .Select(n => n.AuthorId)
                .Distinct()
                .ToList();

            if (authorIds.Count != 0)
            {
                var authors = (await _userRepository.GetAsync(u => authorIds.Contains(u.Id))).ToDictionary(u => u.Id);

                foreach (var news in newsList)
                {
                    if (authors.TryGetValue(news.AuthorId, out var author))
                    {
                        news.Author = author;
                    }
                }
            }

            // Batch load Comments
            var newsIds = newsList.Select(n => n.Id).ToList();
            var allComments = (await _commentRepository.GetAsync(c => newsIds.Contains(c.NewsId))).ToList();

            if (allComments.Count != 0)
            {
                var commentAuthorIds = allComments
                    .Where(c => c.AuthorId != ObjectId.Empty)
                    .Select(c => c.AuthorId)
                    .Distinct()
                    .ToList();

                if (commentAuthorIds.Count != 0)
                {
                    var commentAuthors = (await _userRepository.GetAsync(u => commentAuthorIds.Contains(u.Id))).ToDictionary(u => u.Id);
                    foreach (var comment in allComments)
                    {
                        if (commentAuthors.TryGetValue(comment.AuthorId, out var author))
                        {
                            comment.Author = author;
                        }
                    }
                }
            }

            var commentsGroupedByNews = allComments.GroupBy(c => c.NewsId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var news in newsList)
            {
                if (commentsGroupedByNews.TryGetValue(news.Id, out var comments))
                {
                    news.Comments = comments;
                }
                else
                {
                    news.Comments = new List<Comment>();
                }
            }
        }

        var newsModels = newsList.Adapt<IEnumerable<NewsModel>>().ToList();

        foreach (var newsModel in newsModels)
        {
            await LoadRelatedData(newsModel);
        }

        return new PagedResult<NewsModel>
        {
            Items = newsModels,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    /// <summary>
    /// Loads related author and comment data for a news model.
    /// Uses batch loading for comment authors to optimize database queries.
    /// </summary>
    /// <param name="newsModel">The news model to populate with related data.</param>
    private async Task LoadRelatedData(NewsModel newsModel)
    {
        if (ObjectId.TryParse(newsModel.AuthorId, out var authorId))
        {
            var author = await _userRepository.GetByIdAsync(authorId);
            newsModel.Author = author.Adapt<UserModel>();
        }

        if (ObjectId.TryParse(newsModel.Id, out var newsId))
        {
            var comments = (await _commentRepository.GetAsync(c => c.NewsId == newsId)).ToList();
            if (comments.Count != 0)
            {
                var commentAuthorIds = comments
                    .Where(c => c.AuthorId != ObjectId.Empty)
                    .Select(c => c.AuthorId)
                    .Distinct()
                    .ToList();

                if (commentAuthorIds.Count != 0)
                {
                    var authors = (await _userRepository.GetAsync(u => commentAuthorIds.Contains(u.Id))).ToDictionary(u => u.Id);
                    foreach (var comment in comments)
                    {
                        if (authors.TryGetValue(comment.AuthorId, out var author))
                        {
                            comment.Author = author;
                        }
                    }
                }
            }
            newsModel.Comments = comments.Adapt<ICollection<CommentModel>>();

            foreach (var commentModel in newsModel.Comments)
            {
                if (commentModel.AuthorId is not null && ObjectId.TryParse(commentModel.AuthorId, out var commentAuthorId))
                {
                    var commentAuthor = await _userRepository.GetByIdAsync(commentAuthorId);
                    commentModel.Author = commentAuthor.Adapt<UserModel>();
                }
            }
        }
    }
}
