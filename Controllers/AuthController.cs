using BookmarkManagerApp.Controllers.Requests;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> RegisterAsync(RegisterDataRequest registerData)
    {
        registerData.Validate();
        var user = await authService.RegisterUserAsync(registerData.ToCommand());
        return CreatedAtAction(null, new { id = user.UserId }, user);
    }
}