using BookmarkManagerApp.Models;
using BookmarkManagerApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManagerApp.Repositories;

public class UserRepository(BookmarkDbContext context)
{
    public async Task<bool> ExistsByEmailAsync(string email) =>
        await context.Users.AsNoTracking().AnyAsync(x => x.Email == email);

    public async Task<List<User>> RetrieveAllAsync() => await context.Users.AsNoTracking().ToListAsync();

    public async Task<User> CreateAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> RetrieveByEmailAsync(string email) =>
        await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

    public async Task<User?> RetrieveByIdAsync(long userId) =>
        await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
}