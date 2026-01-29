using BookmarkManagerApp.Repositories;
using BookmarkManagerApp.Services.Utils;

namespace BookmarkManagerApp.Services;

public class BookmarkTagService(BookmarkTagRepository bookmarkTagRepository, UserContext userContext)
{
    public async Task<IEnumerable<BookmarkTagCount>> RetrieveAllBookmarkTagCountAsync()
    {
        return await bookmarkTagRepository.RetrieveAllCountByUserIdAsync(userContext.UserId);
    }
}