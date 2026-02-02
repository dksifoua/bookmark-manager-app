using bookmark_manager_app.Exceptions;
using bookmark_manager_app.Models;
using bookmark_manager_app.Repositories;

namespace bookmark_manager_app.Services;

public class VisitService(VisitRepository visitRepository)
{
    public async Task<Visit> CreateAsync(Visit visit)
    {
        if (await visitRepository.ExistsByBookmarkIdAndCreationTime(visit.BookmarkId, visit.VisitTime))
        {
            throw new ConflictException("A visit already exists for this bookmark and time");
        }

        return await visitRepository.CreateAsync(visit);
    }

    public async Task<Visit> GetByIdAsync(long visitId) => 
        await visitRepository.GetByIdAsync(visitId) ?? throw new NotFoundException("Visit not found");
}