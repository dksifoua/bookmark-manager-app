using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories;
using BookmarkManagerApp.Services.Utils;

namespace BookmarkManagerApp.Services;

public class UserService(UserRepository userRepository, UserContext userContext)
{
    public async Task<IEnumerable<User>> GetAllUsersAsync() => await userRepository.RetrieveAllAsync();
    
    public async Task<User?> GetUserByIdAsync(long userId) => await userRepository.RetrieveByIdAsync(userId);

    public async Task<User?> GetCurrentUserAsync() => await userRepository.RetrieveByIdAsync(userContext.UserId);
}