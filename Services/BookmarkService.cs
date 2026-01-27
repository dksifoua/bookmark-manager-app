using System.Security.Claims;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories;
using BookmarkManagerApp.Services.Utils;

namespace BookmarkManagerApp.Services;

public class BookmarkService(BookmarkRepository bookmarkRepository, UserContext userContext)
{
    public async Task<IEnumerable<Bookmark>> GetAllBookmarksAsync()
    {
        return await bookmarkRepository.GetAllBookmarksByIdAsync(userContext.UserId);
    }
}