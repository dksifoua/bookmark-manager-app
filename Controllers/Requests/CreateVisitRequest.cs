namespace bookmark_manager_app.Controllers.Requests;

public record CreateVisitRequest(long BookmarkId, DateTimeOffset VisitTime);