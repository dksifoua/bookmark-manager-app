using BookmarkManagerApp.Models;
using BookmarkManagerApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController(UserService userService) : ControllerBase
{
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllAsync() 
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }
}