using bookmark_manager_app.Models;
using bookmark_manager_app.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Repositories;

public class VisitRepository(BookmarkDbContext context)
{
    public async Task<bool> ExistsByBookmarkIdAndCreationTime(long bookmarkId, DateTimeOffset creationTime) =>
        await context.Visits.AsNoTracking().AnyAsync(x => x.BookmarkId == bookmarkId && x.VisitTime == creationTime);
    
    public async Task<Visit?> GetByIdAsync(long visitId) =>
        await context.Visits.AsNoTracking().FirstOrDefaultAsync(x => x.VisitId == visitId);
    
    public async Task<Visit> CreateAsync(Visit visit)
    {
        await context.Visits.AddAsync(visit);
        await context.SaveChangesAsync();
        return visit;
    }
}