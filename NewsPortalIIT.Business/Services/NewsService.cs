using Mapster;
using MongoDB.Bson;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;
using NewsPortalIIT.Domain.Repositories;

namespace NewsPortalIIT.Business.Services;

public class NewsService : INewsService
{
    private readonly IRepository<News> _newsRepository;

    public NewsService(IRepository<News> newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<IEnumerable<NewsModel>> GetAllAsync()
    {
        var news = await _newsRepository.GetAllAsync(x => x.Author, x => x.Comments);
        return news.Adapt<IEnumerable<NewsModel>>();
    }

    public async Task<NewsModel> GetByIdAsync(string id)
    {
        var news = await _newsRepository.GetByIdAsync(ObjectId.Parse(id), x => x.Author, x => x.Comments);
        return news.Adapt<NewsModel>();
    }

    public async Task CreateAsync(NewsModel newsModel)
    {
        var news = newsModel.Adapt<News>();
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
}
