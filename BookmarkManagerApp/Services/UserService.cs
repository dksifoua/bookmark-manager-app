using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories.Contracts;

namespace BookmarkManagerApp.Services;

public class UserService(IUserRepository userRepository)
{
    public async Task<User> GetUserByIdAsync(long userId) =>
        await userRepository.GetByIdAsync(userId) ?? throw new NotFoundException("User ID not found");
}