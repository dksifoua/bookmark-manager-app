using BookmarkManagerApp.Models;

namespace BookmarkManagerApp.Controllers.Responses;

public record VisitResponse(long BookmarkId, DateTimeOffset VisitTime)
{
    public static VisitResponse FromModel(Visit visit) => new(visit.BookmarkId, visit.VisitTime.ToUniversalTime());
}