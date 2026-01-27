using BookmarkManagerApp.Controllers.Requests;
using BookmarkManagerApp.Controllers.Responses;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController(
    AuthService authService,
    IConfiguration configuration,
    IValidator<UserRegistrationRequest> userRegistrationValidator,
    IValidator<UserLoginRequest> userLoginValidator)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> RegisterAsync(UserRegistrationRequest userRegistrationRequest)
    {
        await userRegistrationValidator.ValidateAndThrowAsync(userRegistrationRequest);
        var user = await authService.RegisterUserAsync(userRegistrationRequest.ToCommand());
        return CreatedAtAction(null, new { id = user.UserId }, user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResponse>> LoginAsync(UserLoginRequest userLoginRequest)
    {
        await userLoginValidator.ValidateAndThrowAsync(userLoginRequest);
        
        var jwtToken = await authService.AuthenticateUserAsync(userLoginRequest.ToCommand());
        var durationInMinutes = configuration.GetValue("JwtSettings:DurationInMinutes", 5);
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            MaxAge = TimeSpan.FromMinutes(durationInMinutes)
        };
        
        Response.Cookies.Append("token", jwtToken.Token, cookieOptions);
        return Ok(new UserLoginResponse(jwtToken.Fullname, jwtToken.Email));
    }
    
    [HttpPost("logout")]
    public ActionResult Logout()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Path = "/"
        };
        Response.Cookies.Delete("token", cookieOptions);
        return Ok();
    }
}