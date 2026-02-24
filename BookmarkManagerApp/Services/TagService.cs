using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories;
using BookmarkManagerApp.Repositories.Contracts;

namespace BookmarkManagerApp.Services;

public class TagService(ITagRepository tagRepository)
{
    public async Task<IEnumerable<Tag>> GetTagsAsync()
    {
        return await tagRepository.GetAllAsync();
    }
}