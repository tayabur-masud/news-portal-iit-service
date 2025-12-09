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
        var data = GetDbData();
        return data.Users ?? new List<User>();
    }

    [HttpGet("{id}")]
    public User? Get(int id)
    {
        var data = GetDbData();
        return data.Users?.FirstOrDefault(n => n.Id == id);
    }

    [HttpPost]
    public void Post([FromBody] User model)
    {
        var data = GetDbData();

        if (data.Users == null) data.Users = new List<User>();

        // Generate ID
        int userId = data.Users.Count != 0 ? data.Users.Max(n => n.Id) + 1 : 1;
        model.Id = userId;

        data.Users.Add(model);

        SaveDbData(data);
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] User model)
    {
        var data = GetDbData();
        if (data.Users == null) return;

        var existingUser = data.Users.FirstOrDefault(n => n.Id == id);
        if (existingUser != null)
        {
            existingUser.Name = model.Name;
            existingUser.Email = model.Email;

            SaveDbData(data);
        }
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        var data = GetDbData();
        if (data.Users == null) return;

        var userToDelete = data.Users.FirstOrDefault(n => n.Id == id);
        if (userToDelete != null)
        {
            data.Users.Remove(userToDelete);
            SaveDbData(data);
        }
    }
}
