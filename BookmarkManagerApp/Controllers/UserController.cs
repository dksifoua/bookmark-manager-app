using BookmarkManagerApp.Controllers.Responses;
using BookmarkManagerApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Controllers;

[ApiController]
[Route("/api/users")]
[Authorize]
public class UserController(UserService userService) : ControllerBase
{
    [HttpGet("{id:long}", Name = nameof(GetUserByIdAsync))]
    public async Task<ActionResult<GetUserByIdResponse>> GetUserByIdAsync(long id)
    {
        var user = await userService.GetUserByIdAsync(id);
        return Ok(GetUserByIdResponse.FromModel(user));
    }
}