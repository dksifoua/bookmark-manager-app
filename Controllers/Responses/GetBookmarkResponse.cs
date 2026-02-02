using bookmark_manager_app.Models;

namespace bookmark_manager_app.Controllers.Responses;

public record GetBookmarkResponse(
    long? BookmarkId,
    string Title,
    string Url,
    string Description,
    bool IsPinned,
    bool IsArchived,
    string[] Tags,
    DateTimeOffset CreationTime,
    int VisitsCount,
    DateTimeOffset? LastVisitTime
)
{
    public static GetBookmarkResponse FromModel(Bookmark bookmark) => new(
        bookmark.BookmarkId,
        bookmark.Title,
        bookmark.Url,
        bookmark.Description,
        bookmark.IsPinned,
        bookmark.IsArchived,
        bookmark.Tags
            .Select(t => t.Name)
            .ToArray(),
        bookmark.CreationTime,
        bookmark.Visits.Count,
        bookmark.Visits.OrderByDescending(v => v.VisitTime).FirstOrDefault()?.VisitTime
    );
}