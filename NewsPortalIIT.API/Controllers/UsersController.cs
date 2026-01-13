using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortalIIT.API.Models;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Business.Services;

namespace NewsPortalIIT.API.Controllers;

[Route("api/users")]
[Consumes("application/json")]
[ApiController]
[AllowAnonymous]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    public async Task<IEnumerable<UserResponse>> Get()
    {
        var users = await _userService.GetAllAsync();
        return users.Adapt<IEnumerable<UserResponse>>();
    }

    [HttpGet("{id}")]
    public async Task<UserResponse> Get(string id)
    {
        var userModel = await _userService.GetByIdAsync(id);
        return userModel.Adapt<UserResponse>();
    }

    [HttpPost]
    public async Task Post([FromBody] UserRequest model)
    {
        await _userService.CreateAsync(model.Adapt<UserModel>());
    }

    [HttpPut("{id}")]
    public async Task Put(string id, [FromBody] UserRequest model)
    {
        var userModel = model.Adapt<UserModel>();
        userModel.Id = id;
        await _userService.UpdateAsync(userModel);
    }

    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        await _userService.DeleteAsync(id);
    }
}
