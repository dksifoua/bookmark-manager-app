using BookmarkManagerApp.Controllers.Responses;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Controllers;

[ApiController]
[Authorize]
[Route("/api/users")]
public class UserController(UserService userService) : ControllerBase
{
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync() 
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("me")]
    public async Task<ActionResult<GetCurrentUserResponse>> GetCurrentUserAsync()
    {
        var user = await userService.GetCurrentUserAsync();
        if (user == null)
        {
            return NotFound();
        }
        return Ok(GetCurrentUserResponse.FromModel(user));
    }

    [HttpGet("{id:long}", Name = nameof(GetUserByIdAsync))]
    public async Task<ActionResult<User>> GetUserByIdAsync(long id)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}