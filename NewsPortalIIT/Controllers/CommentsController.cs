using Microsoft.AspNetCore.Mvc;
using NewsPortalIIT.Models;

namespace NewsPortalIIT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : BaseController
{
    [HttpGet]
    public IEnumerable<Comment> Get()
    {
        var data = GetDbData();
        return data.Comments ?? new List<Comment>();
    }

    [HttpGet("{id}")]
    public Comment? Get(int id)
    {
        var data = GetDbData();
        return data.Comments?.FirstOrDefault(n => n.Id == id);
    }

    [HttpPost]
    public void Post([FromBody] Comment model)
    {
        var data = GetDbData();

        if (data.Comments == null) data.Comments = new List<Comment>();

        // Generate ID
        int newId = data.Comments.Count != 0 ? data.Comments.Max(n => n.Id) + 1 : 1;
        model.Id = newId;
        model.CreatedAt = DateTime.UtcNow; // Ensure a date is set if not provided

        data.Comments.Add(model);

        SaveDbData(data);
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Comment model)
    {
        var data = GetDbData();
        if (data.Comments == null) return;

        var existingComment = data.Comments.FirstOrDefault(n => n.Id == id);
        if (existingComment != null)
        {
            existingComment.Text = model.Text;
            existingComment.AuthorId = model.AuthorId;
            existingComment.NewsId = model.NewsId;

            SaveDbData(data);
        }
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        var data = GetDbData();
        if (data.Comments == null) return;

        var commentToDelete = data.Comments.FirstOrDefault(n => n.Id == id);
        if (commentToDelete != null)
        {
            data.Comments.Remove(commentToDelete);
            SaveDbData(data);
        }
    }
}
