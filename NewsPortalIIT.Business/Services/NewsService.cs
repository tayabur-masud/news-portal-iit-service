using Mapster;
using MongoDB.Bson;
using System.Linq.Expressions;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;
using NewsPortalIIT.Domain.Repositories;
using NewsPortalIIT.Domain.UnitOfWork;

namespace NewsPortalIIT.Business.Services;

public class NewsService : INewsService
{
    private readonly IRepository<News> _newsRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Comment> _commentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NewsService(
        IRepository<News> newsRepository,
        IRepository<User> userRepository,
        IRepository<Comment> commentRepository,
        IUnitOfWork unitOfWork)
    {
        _newsRepository = newsRepository;
        _userRepository = userRepository;
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
    }

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

    public async Task CreateAsync(NewsModel newsModel)
    {
        var news = newsModel.Adapt<News>();
        news.CreatedAt = DateTime.UtcNow;
        await _newsRepository.AddAsync(news);
    }

    public async Task UpdateAsync(NewsModel newsModel)
    {
        var newsFromDb = await _newsRepository.GetByIdAsync(ObjectId.Parse(newsModel.Id));

        if (newsFromDb is null)
        {
            throw new Exception("News not found");
        }

        var news = newsModel.Adapt<News>();
        news.CreatedAt = newsFromDb.CreatedAt;

        await _newsRepository.UpdateAsync(news);
    }

    public async Task DeleteAsync(string id)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _commentRepository.DeleteAsync(c => c.NewsId == ObjectId.Parse(id));
            await _newsRepository.DeleteAsync(ObjectId.Parse(id));
            await _unitOfWork.CommitAsync();
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<PagedResult<NewsModel>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        Expression<Func<News, bool>> predicate = n => true;
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            predicate = n => n.Title.Contains(searchTerm);
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

            if (allComments.Any())
            {
                var commentAuthorIds = allComments
                    .Where(c => c.AuthorId != ObjectId.Empty)
                    .Select(c => c.AuthorId)
                    .Distinct()
                    .ToList();

                if (commentAuthorIds.Any())
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

        return new PagedResult<NewsModel>
        {
            Items = newsModels,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

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
            if (comments.Any())
            {
                var commentAuthorIds = comments
                    .Where(c => c.AuthorId != ObjectId.Empty)
                    .Select(c => c.AuthorId)
                    .Distinct()
                    .ToList();

                if (commentAuthorIds.Any())
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

            foreach(var commentModel in newsModel.Comments)
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
