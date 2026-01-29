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
        return await context.Tags.AsNoTracking().Where(t => names.Contains(t.Name)).ToListAsync();
    }

    public async Task<IEnumerable<Tag>> RetrieveAllByUserIdAsync(long userId)
    {
        return await context.Tags
            .AsNoTracking()
            .Where(t => t.BookmarkTags.Any(bt => bt.Bookmark!.UserId == userId))
            .Distinct()
            .ToListAsync();
    }
}