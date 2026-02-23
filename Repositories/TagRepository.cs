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

    public async Task<IEnumerable<Tag>> GetAllAsync() =>
        await context.Tags.AsNoTracking().ToListAsync();
}