using BookmarkManagerApp.Models;

namespace BookmarkManagerApp.Repositories.Contracts;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(long userId);
    Task<User> CreateAsync(User user);
    Task<bool> EmailExistsAsync(string email);
}