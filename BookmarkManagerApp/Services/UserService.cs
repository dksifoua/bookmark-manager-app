using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories;

namespace BookmarkManagerApp.Services;

public class UserService (UserRepository userRepository)
{
    public async Task<User> GetUserByIdAsync(long userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        return user ?? throw new NotFoundException("User ID not found");
    }
}