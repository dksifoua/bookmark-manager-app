using BookmarkManagerApp.Models;
using BookmarkManagerApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManagerApp.Repositories;

public class UserRepository(BookmarkDbContext context)
{
    public async Task<bool> ExistsByEmailAsync(string email) => await context.Set<User>().AnyAsync(x => x.Email == email);
    
    public async Task<List<User>> GetAllAsync() => await context.Users.AsNoTracking().ToListAsync();
    
    public async Task<User> CreateAsync(User user)
    {
        await context.Set<User>().AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }
    
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Set<User>().FirstOrDefaultAsync(x => x.Email == email);
    }
    
    public async Task<User?> GetByIdAsync(long userId)
    {
        return await context.Set<User>().FirstOrDefaultAsync(x => x.UserId == userId);
    }
}