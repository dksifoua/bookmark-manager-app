using bookmark_manager_app.Models;
using bookmark_manager_app.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Repositories;

public class BookmarkRepository(BookmarkDbContext context)
{
    public async Task<IEnumerable<Bookmark>> GetAllByUserIdAndSearchTermAsync(long userId, string searchTerm) =>
        await context.Bookmarks.AsNoTracking()
            // ReSharper disable once EntityFramework.UnsupportedServerSideFunctionCall
            .Where(b => b.UserId == userId && b.SearchVector.Matches(EF.Functions.WebSearchToTsQuery(searchTerm)))
            .Include(bt => bt.Tags)
            .Include(bt => bt.Visits)
            .ToListAsync();

    public async Task<bool> ExistsByBookmarkId(long bookmarkId) =>
        await context.Bookmarks.AsNoTracking().AnyAsync(x => x.BookmarkId == bookmarkId);

    public async Task<bool> ExistsByUserIdAndTitle(long userId, string title) =>
        await context.Bookmarks.AsNoTracking().AnyAsync(x => x.UserId == userId && x.Title == title);

    public async Task<bool> ExistsByUserIdAndUrl(long userId, string url) =>
        await context.Bookmarks.AsNoTracking().AnyAsync(x => x.UserId == userId && x.Url == url);

    public async Task<bool> ExistsByUserIdAndTitleOrUrl(long userId, string title, string url) =>
        await context.Bookmarks.AsNoTracking().AnyAsync(x => x.UserId == userId && (x.Title == title || x.Url == url));

    public async Task<Bookmark> CreateAsync(Bookmark bookmark)
    {
        await context.Bookmarks.AddAsync(bookmark);
        await context.SaveChangesAsync();
        return bookmark;
    }

    public async Task UpdateAsync()
    {
        // The bookmark is excepted to be tracked by EF Core
        await context.SaveChangesAsync();
    }

    public async Task<Bookmark?> GetByIdAsync(long bookmarkId) =>
        await context.Bookmarks.AsNoTracking()
            .Include(x => x.Tags)
            .Include(x => x.Visits)
            .FirstOrDefaultAsync(x => x.BookmarkId == bookmarkId);

    public async Task<Bookmark?> GetByIdForUpdateAsync(long bookmarkId) =>
        await context.Bookmarks
            .Include(x => x.Tags)
            .Include(x => x.Visits)
            .FirstOrDefaultAsync(x => x.BookmarkId == bookmarkId);

    public async Task<IEnumerable<Bookmark>> GetAllByUserIdAsync(long userId) =>
        await context.Bookmarks
            .AsNoTracking()
            .Where(b => b.UserId == userId)
            .Include(bt => bt.Tags)
            .Include(bt => bt.Visits)
            .ToListAsync();

    public async Task TogglePinAsync(long bookmarkId) =>
        await context.Bookmarks
            .Where(b => b.BookmarkId == bookmarkId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.IsPinned, b => !b.IsPinned)
            );

    public async Task ToggleArchiveAsync(long bookmarkId) =>
        await context.Bookmarks
            .Where(b => b.BookmarkId == bookmarkId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.IsArchived, b => !b.IsArchived)
            );

    public async Task DeleteAsync(long bookmarkId) =>
        await context.Bookmarks
            .Where(b => b.BookmarkId == bookmarkId)
            .ExecuteDeleteAsync();
}