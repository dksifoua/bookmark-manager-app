using BookmarkManagerApp.Models;
using JetBrains.Annotations;

namespace BookmarkManagerApp.Controllers.Responses;

public record RetrieveAllBookmarksResponse(
    [UsedImplicitly] long? BookmarkId,
    string Title,
    string Url,
    string Description,
    bool IsPinned,
    bool IsArchived,
    string[] Tags,
    DateTimeOffset CreationTime,
    int VisitCount,
    DateTimeOffset? LastVisitTime
)
{
    public static RetrieveAllBookmarksResponse FromBookmark(Bookmark bookmark) => new(
        bookmark.BookmarkId,
        bookmark.Title,
        bookmark.Url,
        bookmark.Description,
        bookmark.IsPinned,
        bookmark.IsArchived,
        bookmark.BookmarkTags
            .Where(bt => bt.Tag != null)
            .Select(t => t.Tag!.Name)
            .ToArray(),
        bookmark.CreationTime,
        bookmark.Visits.Count,
        bookmark.Visits.OrderByDescending(v => v.VisitTime).FirstOrDefault()?.VisitTime
    );
}