using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories;

namespace BookmarkManagerApp.Services;

public class UserService(UserRepository userRepository)
{
    public async Task<IEnumerable<User>> GetAllUsersAsync() => await userRepository.GetAllAsync();
    
    public async Task<User?> GetUserByIdAsync(long userId) => await userRepository.GetByIdAsync(userId);
}