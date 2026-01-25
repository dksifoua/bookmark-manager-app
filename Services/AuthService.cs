using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories;
using BookmarkManagerApp.Services.Commands;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BookmarkManagerApp.Services;

public class AuthService(
    UserRepository userRepository,
    IConfiguration configuration,
    PasswordHasher<IdentityUser> passwordHasher)
{
    public async Task<User> RegisterUserAsync(RegisterUserCommand command)
    {
        if (await userRepository.ExistsByEmailAsync(command.Email))
        {
            throw new BadRequestException($"User with email '{command.Email}' already exists.");
        }

        var hashedPassword = await HashPassword(command.Password);
        var user = new User { FullName = command.FullName, Email = command.Email, Password = hashedPassword };
        return await userRepository.CreateAsync(user);
    }

    public async Task<IDictionary<string, string>> AuthenticateUserAsync(LoginUserCommand command)
    {
        var user = await userRepository.GetByEmailAsync(command.Email);
        if (user == null) throw new ResourceNotFoundException($"User with email '{command.Email}' not found.");

        var isPasswordValid = await VerifyHashedPassword(user.Password, command.Password);
        if (!isPasswordValid) throw new BadRequestException("Invalid credentials.");

        return await GenerateJwtToken(user);
    }

    private async Task<string> HashPassword(string password) =>
        await Task.FromResult(passwordHasher.HashPassword(new IdentityUser(), password));

    private async Task<bool> VerifyHashedPassword(string hashedPassword, string providedPassword) =>
        await Task.FromResult(
            passwordHasher.VerifyHashedPassword(new IdentityUser(), hashedPassword, providedPassword) ==
            PasswordVerificationResult.Success);

    private async Task<IDictionary<string, string>> GenerateJwtToken(User user)
    {
        var secretKey = configuration["JwtSettings:Key"] ?? string.Empty;
        var issuer = configuration["JwtSettings:Issuer"];
        var audience = configuration["JwtSettings:Audience"];
        var durationInMinutes = configuration.GetValue("JwtSettings:DurationInMinutes", 5);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
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

        return await Task.FromResult<IDictionary<string, string>>(new Dictionary<string, string>
        {
            { "token", tokenHandler.WriteToken(token) },
            { "expires", tokenDescriptor.Expires.ToString() ?? string.Empty }
        });
    }
}