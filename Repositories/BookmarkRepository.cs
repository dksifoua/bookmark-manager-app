using BookmarkManagerApp.Models;
using BookmarkManagerApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManagerApp.Repositories;

public class BookmarkRepository(BookmarkDbContext context)
{
    public async Task<IEnumerable<Bookmark>> GetAllBookmarksByIdAsync(long userId)
    {
        return await context.Bookmarks.AsNoTracking().Where(b => b.UserId == userId).ToListAsync();
    }
}