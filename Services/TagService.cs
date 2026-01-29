using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories;
using BookmarkManagerApp.Services.Utils;

namespace BookmarkManagerApp.Services;

public class TagService(TagRepository tagRepository, UserContext userContext)
{
    public async Task<IEnumerable<Tag>> RetrieveAllTagsByUserIdAsync()
    {
        return await tagRepository.RetrieveAllByUserIdAsync(userContext.UserId);
    }
}