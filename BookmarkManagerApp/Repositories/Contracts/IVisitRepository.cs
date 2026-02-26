using BookmarkManagerApp.Models;

namespace BookmarkManagerApp.Repositories.Contracts;

public interface IVisitRepository
{
    Task<bool> ExistsByBookmarkIdAndCreationTime(long bookmarkId, DateTimeOffset creationTime);
    Task<Visit?> GetByIdAsync(long visitId);
    Task<Visit> CreateAsync(Visit visit);
}