using BookmarkManagerApp.Controllers.Requests;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController(AuthService authService, IValidator<UserRegistrationRequest> userRegistrationValidator)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> RegisterAsync(UserRegistrationRequest userRegistrationRequest)
    {
        await userRegistrationValidator.ValidateAndThrowAsync(userRegistrationRequest);
        var user = await authService.RegisterUserAsync(userRegistrationRequest.ToCommand());
        return CreatedAtAction(null, new { id = user.UserId }, user);
    }
}