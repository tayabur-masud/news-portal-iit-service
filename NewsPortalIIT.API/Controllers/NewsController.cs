using Mapster;
using Microsoft.AspNetCore.Mvc;
using NewsPortalIIT.API.Models;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Business.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NewsPortalIIT.API.Controllers;

[Route("api/news")]
[Consumes("application/json")]
[ApiController]
public class NewsController(INewsService newsService) : ControllerBase
{
    private readonly INewsService _newsService = newsService;

    [HttpGet]
    public async Task<PagedResponse<NewsResponse>> Get(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        var result = await _newsService.GetPagedAsync(pageNumber, pageSize, searchTerm);

        var response = new PagedResponse<NewsResponse>
        {
            Items = result.Items.Adapt<IEnumerable<NewsResponse>>(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalPages = result.TotalPages
        };

        return response;
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
        var newsModel = model.Adapt<NewsModel>();
        newsModel.AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? throw new UnauthorizedAccessException("AuthorId not found in token");
        await _newsService.CreateAsync(newsModel);
    }

    [HttpPut("{id}")]
    public async Task Put(string id, [FromBody] NewsRequest model)
    {
        var newsModel = model.Adapt<NewsModel>();
        newsModel.Id = id;
        newsModel.AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? throw new UnauthorizedAccessException("AuthorId not found in token");
        await _newsService.UpdateAsync(newsModel);
    }

    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        await _newsService.DeleteAsync(id);
    }
}
