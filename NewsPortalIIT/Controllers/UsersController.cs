using Microsoft.AspNetCore.Mvc;
using NewsPortalIIT.Models;

namespace NewsPortalIIT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : BaseController
{
    [HttpGet]
    public IEnumerable<User> Get()
    {
        return new List<User> { new User() };
    }

    [HttpGet("{id}")]
    public User Get(int id)
    {
        return new User();
    }

    [HttpPost]
    public void Post([FromBody] User model)
    {
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] User model)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
