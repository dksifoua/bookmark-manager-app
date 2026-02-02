using bookmark_manager_app.Controllers.Responses;
using bookmark_manager_app.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController(UserService userService) : ControllerBase
{
    [HttpGet("{id:long}", Name = nameof(GetUserByIdAsync))]
    public async Task<ActionResult<GetUserByIdResponse>> GetUserByIdAsync(long id)
    {
        var user = await userService.GetUserByIdAsync(id);
        return Ok(GetUserByIdResponse.FromModel(user));
    }
}