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
        return new List<Comment> {new Comment() };
    }

    [HttpGet("{id}")]
    public Comment Get(int id)
    {
        return new Comment();
    }

    [HttpPost]
    public void Post([FromBody] Comment model)
    {
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Comment model)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
