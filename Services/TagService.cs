using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories;

namespace BookmarkManagerApp.Services;

public class TagService(TagRepository tagRepository)
{
    public async Task<IEnumerable<Tag>> GetTagsAsync()
    {
        return await tagRepository.GetAllAsync();
    }
}