using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using bookmark_manager_app.Exceptions;
using bookmark_manager_app.Models;
using bookmark_manager_app.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace bookmark_manager_app.Services;

public class AuthService(
    UserRepository userRepository,
    PasswordHasher<IdentityUser> passwordHasher,
    IConfiguration configuration)
{
    public async Task<User> RegisterAsync(string fullname, string email, string password)
    {
        if (await userRepository.EmailExistsAsync(email.ToLower()))
            throw new ConflictException($"Email {email} already exists");

        var hashedPassword = await HashPassword(password);
        return await userRepository.CreateAsync(new User
            { Email = email.ToLower(), Fullname = fullname, Password = hashedPassword });
    }

    public async Task<JwtToken> AuthenticateUserAsync(string email, string password)
    {
        var user = await userRepository.GetByEmailAsync(email.ToLower());
        if (user == null || !await VerifyHashedPassword(user.Password, password))
        {
            throw new UnauthorizedException("Email or password is incorrect.");
        }

        return await GenerateJwtToken(user);
    }

    private async Task<string> HashPassword(string password) =>
        await Task.FromResult(passwordHasher.HashPassword(new IdentityUser(), password));

    private async Task<bool> VerifyHashedPassword(string hashedPassword, string providedPassword) =>
        await Task.FromResult(
            passwordHasher.VerifyHashedPassword(new IdentityUser(), hashedPassword, providedPassword) ==
            PasswordVerificationResult.Success);

    private async Task<JwtToken> GenerateJwtToken(User user)
    {
        var secretKey = configuration["Jwt:Key"] ?? string.Empty;
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];
        var durationInMinutes = configuration.GetValue("Jwt:DurationInMinutes", 5);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString() ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Fullname),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(durationInMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return await Task.FromResult(new JwtToken(tokenHandler.WriteToken(token), user.Fullname, user.Email));
    }
}

public record JwtToken(string Token, string Fullname, string Email);