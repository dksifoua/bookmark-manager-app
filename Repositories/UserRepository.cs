using bookmark_manager_app.Models;
using bookmark_manager_app.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Repositories;

public class UserRepository(BookmarkDbContext context)
{
    public async Task<User?> GetByEmailAsync(string email) =>
        await context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByIdAsync(long userId) =>
        await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);

    public async Task<User> CreateAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await context.Users.AsNoTracking()
            .AnyAsync(u => u.Email == email);
    }
}