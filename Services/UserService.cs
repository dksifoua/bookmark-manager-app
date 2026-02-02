using bookmark_manager_app.Exceptions;
using bookmark_manager_app.Models;
using bookmark_manager_app.Repositories;

namespace bookmark_manager_app.Services;

public class UserService (UserRepository userRepository)
{
    public async Task<User> GetUserByIdAsync(long userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        return user ?? throw new NotFoundException("User ID not found");
    }
}