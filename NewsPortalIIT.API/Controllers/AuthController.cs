using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortalIIT.API.Models;
using NewsPortalIIT.API.Services;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Business.Services;

namespace NewsPortalIIT.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            var user = await _userService.LoginAsync(loginRequest);
            var token = _tokenService.GenerateToken(user);

            return Ok(new AuthResponse
            {
                User = user.Adapt<UserResponse>(),
                Token = token
            });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
