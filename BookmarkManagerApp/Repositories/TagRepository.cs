using BookmarkManagerApp.Models;
using BookmarkManagerApp.Persistence;
using BookmarkManagerApp.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManagerApp.Repositories;

public class TagRepository(BookmarkDbContext context) : ITagRepository
{
    public async Task<IEnumerable<Tag>> GetByNamesForUpdate(IEnumerable<string> names) =>
        await context.Tags.Where(t => names.Contains(t.Name)).ToListAsync();

    public async Task<IEnumerable<Tag>> GetAllAsync() =>
        await context.Tags.AsNoTracking().ToListAsync();
}