using BookmarkManagerApp.Controllers.Requests;
using BookmarkManagerApp.Controllers.Responses;
using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    AuthService authService,
    IValidator<UserRegistrationRequest> userRegistrationRequestValidator,
    IValidator<UserLoginRequest> userLoginRequestValidator,
    IConfiguration configuration,
    IWebHostEnvironment env) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserRegistrationResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<UserRegistrationResponse>> RegisterUserAsync(
        UserRegistrationRequest userRegistrationRequest)
    {
        await userRegistrationRequestValidator.ValidateAndThrowAsync(userRegistrationRequest);

        var user = await authService.RegisterAsync(userRegistrationRequest.Fullname, userRegistrationRequest.Email,
            userRegistrationRequest.Password);
        return CreatedAtRoute(nameof(UserController.GetUserByIdAsync), new { id = user.UserId },
            new UserRegistrationResponse(user.Fullname, user.Email));
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserLoginResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<UserLoginResponse>> LoginAsync(UserLoginRequest userLoginRequest)
    {
        await userLoginRequestValidator.ValidateAndThrowAsync(userLoginRequest);

        var jwtToken = await authService.AuthenticateUserAsync(userLoginRequest.Email, userLoginRequest.Password);
        var durationInMinutes = configuration.GetValue("Jwt:DurationInMinutes", 5);
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = !env.IsDevelopment(),
            SameSite = env.IsDevelopment() ? SameSiteMode.Strict : SameSiteMode.None,
            MaxAge = TimeSpan.FromMinutes(durationInMinutes)
        };

        Response.Cookies.Append("token", jwtToken.Token, cookieOptions);
        return Ok(new UserLoginResponse(jwtToken.Fullname, jwtToken.Email));
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult Logout()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = !env.IsDevelopment(),
            SameSite = env.IsDevelopment() ? SameSiteMode.Strict : SameSiteMode.None,
            Path = "/"
        };
        Response.Cookies.Delete("token", cookieOptions);
        return NoContent();
    }
}