using Mapster;
using MongoDB.Bson;
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

    public async Task<IEnumerable<NewsModel>> GetAllAsync()
    {
        var newsList = (await _newsRepository.GetAllAsync()).ToList();
        var newsModels = newsList.Adapt<IEnumerable<NewsModel>>().ToList();

        if (newsModels.Count == 0) return newsModels;

        // Batch load Authors
        var authorIds = newsList
            .Where(n => n.AuthorId != ObjectId.Empty)
            .Select(n => n.AuthorId)
            .Distinct()
            .ToList();

        var authors = await _userRepository.GetAsync(u => authorIds.Contains(u.Id));
        var authorMap = authors.Adapt<IEnumerable<UserModel>>().ToDictionary(a => a.Id);

        // Batch load Comments
        var newsIds = newsList.Select(n => n.Id).ToList();
        var allComments = await _commentRepository.GetAsync(c => newsIds.Contains(c.NewsId));
        var commentModels = allComments.Adapt<IEnumerable<CommentModel>>().ToList();
        var commentsGroupedByNews = commentModels.GroupBy(c => c.NewsId).ToDictionary(g => g.Key, g => g.ToList());

        // Assign related data
        foreach (var newsModel in newsModels)
        {
            if (authorMap.TryGetValue(newsModel.AuthorId, out var author))
            {
                newsModel.Author = author;
            }

            if (commentsGroupedByNews.TryGetValue(newsModel.Id, out var comments))
            {
                newsModel.Comments = comments;
            }
            else
            {
                newsModel.Comments = new List<CommentModel>();
            }
        }

        return newsModels;
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

    private async Task LoadRelatedData(NewsModel newsModel)
    {
        if (ObjectId.TryParse(newsModel.AuthorId, out var authorId))
        {
            var author = await _userRepository.GetByIdAsync(authorId);
            newsModel.Author = author.Adapt<UserModel>();
        }

        if (ObjectId.TryParse(newsModel.Id, out var newsId))
        {
            var comments = await _commentRepository.GetAsync(c => c.NewsId == newsId);
            newsModel.Comments = comments.Adapt<ICollection<CommentModel>>();
        }
    }
}
