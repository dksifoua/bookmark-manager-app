using bookmark_manager_app.Models;
using bookmark_manager_app.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Repositories;

public class TagRepository(BookmarkDbContext context)
{
    public async Task<bool> ExistsByName(string name) =>
        await context.Tags.AsNoTracking().AnyAsync(t => t.Name == name);

    public async Task<IEnumerable<Tag>> GetByNamesForUpdate(IEnumerable<string> names) =>
        await context.Tags.Where(t => names.Contains(t.Name)).ToListAsync();

    public async Task<IEnumerable<TagCount>> GetCountByUserIdAsync(long userId)
    {
        var result = await context.Tags
            .AsNoTracking()
            .SelectMany(t => t.Bookmarks
                .Where(b => b.UserId == userId)
                .Select(b => new
                {
                    t.TagId,
                    t.Name,
                    b.BookmarkId,
                    b.IsArchived
                }))
            .GroupBy(x => new { x.TagId, x.Name })
            .Select(g => new
            {
                Id = g.Key.TagId,
                g.Key.Name,
                Count = g.Where(x => !x.IsArchived)
                    .Select(x => x.BookmarkId).Distinct().Count(),
                ArchivedCount = g.Where(x => x.IsArchived)
                    .Select(x => x.BookmarkId).Distinct().Count()
            })
            .ToListAsync();

        return result.Select(x => new TagCount(x.Id, x.Name, x.Count, x.ArchivedCount)).ToList();
    }
}

public record TagCount(long? Id, string Name, int Count, int ArchivedCount);