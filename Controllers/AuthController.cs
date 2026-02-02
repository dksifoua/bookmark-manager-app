using bookmark_manager_app.Controllers.Requests;
using bookmark_manager_app.Controllers.Responses;
using bookmark_manager_app.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    AuthService authService,
    IValidator<UserRegistrationRequest> userRegistrationRequestValidator,
    IValidator<UserLoginRequest> userLoginRequestValidator,
    IConfiguration configuration) : ControllerBase
{
    [HttpPost("register")]
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
    public async Task<ActionResult<UserLoginResponse>> LoginAsync(UserLoginRequest userLoginRequest)
    {
        await userLoginRequestValidator.ValidateAndThrowAsync(userLoginRequest);

        var jwtToken = await authService.AuthenticateUserAsync(userLoginRequest.Email, userLoginRequest.Password);
        var durationInMinutes = configuration.GetValue("Jwt:DurationInMinutes", 5);
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