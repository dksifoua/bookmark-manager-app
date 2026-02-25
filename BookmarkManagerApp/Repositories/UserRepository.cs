using BookmarkManagerApp.Models;
using BookmarkManagerApp.Persistence;
using BookmarkManagerApp.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManagerApp.Repositories;

public class UserRepository(BookmarkDbContext context) : IUserRepository
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

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await context.Users.AsNoTracking()
            .AnyAsync(u => u.Email == email);
    }
}