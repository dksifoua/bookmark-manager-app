using BookmarkManagerApp.Models;
using BookmarkManagerApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManagerApp.Repositories;

public class TagRepository(BookmarkDbContext context)
{
    public async Task<Tag> CreateWithoutCommitAsync(Tag tag)
    {
        await context.Tags.AddAsync(tag);
        return tag;
    }
    
    public async Task<IEnumerable<Tag>> RetrieveByNames(IEnumerable<string> names)
    {
        return await context.Tags.Where(t => names.Contains(t.Name)).ToListAsync();
    }
}