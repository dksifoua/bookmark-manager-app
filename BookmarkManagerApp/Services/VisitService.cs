using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories;
using BookmarkManagerApp.Repositories.Contracts;

namespace BookmarkManagerApp.Services;

public class VisitService(IVisitRepository visitRepository)
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