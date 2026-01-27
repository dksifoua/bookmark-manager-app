using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManagerApp.Repositories;

public class BookmarkRepository(BookmarkDbContext context)
{
    public async Task<Bookmark> CreateAsync(Bookmark bookmark)
    {
        await context.Bookmarks.AddAsync(bookmark);
        await context.SaveChangesAsync();
        return bookmark;
    }

    public async Task<bool> ExistsByUserIdAndTitleAndUrl(long userId, string title, string url) =>
        await context.Bookmarks.AsNoTracking().AnyAsync(b => b.UserId == userId && b.Title == title && b.Url == url);

    public async Task DeleteAsync(Bookmark bookmark)
    {
        context.Bookmarks.Remove(bookmark);
        await context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<Bookmark>> RetrieveAllByUserIdAsync(long userId) =>
        await context.Bookmarks.AsNoTracking().Where(b => b.UserId == userId).ToListAsync();
    
    public async Task<Bookmark?> RetrieveByIdAsync(long bookmarkId) =>
        await context.Bookmarks.FindAsync(bookmarkId);
}