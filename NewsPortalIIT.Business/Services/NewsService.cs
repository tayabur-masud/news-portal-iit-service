using Mapster;
using MongoDB.Bson;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;
using NewsPortalIIT.Domain.Repositories;

namespace NewsPortalIIT.Business.Services;

public class NewsService : INewsService
{
    private readonly IRepository<News> _newsRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Comment> _commentRepository;

    public NewsService(
        IRepository<News> newsRepository,
        IRepository<User> userRepository,
        IRepository<Comment> commentRepository)
    {
        _newsRepository = newsRepository;
        _userRepository = userRepository;
        _commentRepository = commentRepository;
    }

    public async Task<IEnumerable<NewsModel>> GetAllAsync()
    {
        var newsList = await _newsRepository.GetAllAsync();
        var newsModels = newsList.Adapt<IEnumerable<NewsModel>>().ToList();

        foreach (var newsModel in newsModels)
        {
            await LoadRelatedData(newsModel);
        }

        return newsModels;
    }

    public async Task<NewsModel> GetByIdAsync(string id)
    {
        var news = await _newsRepository.GetByIdAsync(ObjectId.Parse(id));
        if (news == null) return null;

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
        var news = newsModel.Adapt<News>();
        await _newsRepository.UpdateAsync(news);
    }

    public async Task DeleteAsync(string id)
    {
        await _newsRepository.DeleteAsync(ObjectId.Parse(id));
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
