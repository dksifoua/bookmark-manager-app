using BookmarkManagerApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManagerApp.Repositories;

public class BookmarkTagRepository(BookmarkDbContext context)
{
    public async Task<IEnumerable<BookmarkTagCount>> RetrieveAllCountByUserIdAsync(long userId)
    {
        var result = await context.BookmarkTags
            .AsNoTracking()
            .Where(bt => bt.Bookmark!.UserId == userId)
            .GroupBy(bt => new { bt.TagId, bt.Tag!.Name })
            .Select(g => new
            {
                Id = g.Key.TagId,
                g.Key.Name,
                Count = g.Select(x => x.BookmarkId).Distinct().Count(),
                ArchivedCount = g.Where(x => x.Bookmark!.IsArchived)
                    .Select(x => x.BookmarkId).Distinct().Count()
            })
            .ToListAsync();
        
        return result.Select(x => new BookmarkTagCount(x.Id, x.Name, x.Count, x.ArchivedCount)).ToList();
    }
}

public record BookmarkTagCount(long Id, string Name, int Count, int ArchivedCount);