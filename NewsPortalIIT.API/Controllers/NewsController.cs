using Mapster;
using Microsoft.AspNetCore.Mvc;
using NewsPortalIIT.API.Models;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Business.Services;

namespace NewsPortalIIT.API.Controllers;

[Route("api/news")]
[Consumes("application/json")]
[ApiController]
public class NewsController(INewsService newsService) : ControllerBase
{
    private readonly INewsService _newsService = newsService;

    [HttpGet]
    public async Task<IEnumerable<NewsResponse>> Get()
    {
        var news = await _newsService.GetAllAsync();
        return news.Adapt<IEnumerable<NewsResponse>>();
    }

    [HttpGet("{id}")]
    public async Task<NewsResponse?> Get(string id)
    {
        var news = await _newsService.GetByIdAsync(id);
        return news?.Adapt<NewsResponse>();
    }

    [HttpPost]
    public async Task Post([FromBody] NewsRequest model)
    {
        await _newsService.CreateAsync(model.Adapt<NewsModel>());
    }

    [HttpPut("{id}")]
    public async Task Put(string id, [FromBody] NewsRequest model)
    {
        var newsModel = model.Adapt<NewsModel>();
        newsModel.Id = id;
        await _newsService.UpdateAsync(newsModel);
    }

    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        await _newsService.DeleteAsync(id);
    }
}
