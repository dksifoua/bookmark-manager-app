using bookmark_manager_app.Models;

namespace bookmark_manager_app.Controllers.Responses;

public record VisitResponse(long BookmarkId, DateTimeOffset VisitTime)
{
    public static VisitResponse FromModel(Visit visit) => new(visit.BookmarkId, visit.VisitTime.ToUniversalTime());
}