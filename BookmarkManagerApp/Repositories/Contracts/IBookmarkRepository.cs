using BookmarkManagerApp.Models;

namespace BookmarkManagerApp.Repositories.Contracts;

public interface IBookmarkRepository
{
    Task<IEnumerable<Bookmark>> GetAllByUserIdAndSearchTermAsync(long userId, string searchTerm);
    Task<bool> ExistsByBookmarkId(long bookmarkId);
    Task<bool> ExistsByUserIdAndTitle(long userId, string title);
    Task<bool> ExistsByUserIdAndUrl(long userId, string url);
    Task<bool> ExistsByUserIdAndTitleOrUrl(long userId, string title, string url);
    Task<Bookmark> CreateAsync(Bookmark bookmark);
    Task UpdateAsync();
    Task<Bookmark?> GetByIdAsync(long bookmarkId);
    Task<Bookmark?> GetByIdForUpdateAsync(long bookmarkId);
    Task<IEnumerable<Bookmark>> GetAllByUserIdAsync(long userId);
    Task TogglePinAsync(long bookmarkId);
    Task ToggleArchiveAsync(long bookmarkId);
    Task DeleteAsync(long bookmarkId);
}