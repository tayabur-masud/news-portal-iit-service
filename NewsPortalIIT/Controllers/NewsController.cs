using Microsoft.AspNetCore.Mvc;
using NewsPortalIIT.Models;

namespace NewsPortalIIT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsController : BaseController
{
    [HttpGet]
    public IEnumerable<News> Get()
    {
        var data = GetDbData();
        return data.News ?? new List<News>();
    }

    [HttpGet("{id}")]
    public News? Get(int id)
    {
        var data = GetDbData();
        return data.News?.FirstOrDefault(n => n.Id == id);
    }

    [HttpPost]
    public void Post([FromBody] News model)
    {
        var data = GetDbData();

        if (data.News == null) data.News = new List<News>();

        // Generate ID
        int newId = data.News.Count != 0 ? data.News.Max(n => n.Id) + 1 : 1;
        model.Id = newId;
        model.CreatedAt = DateTime.UtcNow; // Ensure a date is set if not provided

        data.News.Add(model);

        SaveDbData(data);
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] News model)
    {
        var data = GetDbData();
        if (data.News == null) return;

        var existingNews = data.News.FirstOrDefault(n => n.Id == id);
        if (existingNews != null)
        {
            existingNews.Title = model.Title;
            existingNews.Body = model.Body;
            existingNews.AuthorId = model.AuthorId;
            // Preserve CreatedAt and Id from existing record
            // Update Comments only if provided, otherwise preserve existing
            if (model.Comments != null)
            {
                existingNews.Comments = model.Comments;
            }

            SaveDbData(data);
        }
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        var data = GetDbData();
        if (data.News == null) return;

        var newsToDelete = data.News.FirstOrDefault(n => n.Id == id);
        if (newsToDelete != null)
        {
            data.News.Remove(newsToDelete);
            SaveDbData(data);
        }
    }
}
