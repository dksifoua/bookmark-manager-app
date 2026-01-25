using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories;
using BookmarkManagerApp.Services.Commands;
using Microsoft.AspNetCore.Identity;

namespace BookmarkManagerApp.Services;

public class AuthService(UserRepository userRepository, PasswordHasher<IdentityUser> passwordHasher)
{
    public async Task<User> RegisterUserAsync(RegisterUserCommand command)
    {
        if (await userRepository.ExistsByEmailAsync(command.Email))
        {
            throw new ResourceAlreadyExistsException($"User with email '{command.Email}' already exists.");
        }

        var hashedPassword = await HashPassword(command.Password);
        var user = new User { FullName = command.FullName, Email = command.Email, Password = hashedPassword };
        return await userRepository.CreateAsync(user);
    }

    private async Task<string> HashPassword(string password) =>
        await Task.FromResult(passwordHasher.HashPassword(new IdentityUser(), password));

    private async Task<bool> VerifyHashedPassword(string hashedPassword, string providedPassword) =>
        await Task.FromResult(
            passwordHasher.VerifyHashedPassword(new IdentityUser(), hashedPassword, providedPassword) ==
            PasswordVerificationResult.Success);
}