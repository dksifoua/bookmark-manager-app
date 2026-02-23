using bookmark_manager_app.Models;
using bookmark_manager_app.Repositories;
using bookmark_manager_app.Services.Utils;

namespace bookmark_manager_app.Services;

public class TagService(TagRepository tagRepository, UserContext userContext)
{
    public async Task<IEnumerable<Tag>> GetTagsAsync()
    {
        return await tagRepository.GetAllAsync();
    }
}