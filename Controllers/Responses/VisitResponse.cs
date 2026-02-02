namespace bookmark_manager_app.Controllers.Responses;

public record VisitResponse(long BookmarkId, DateTimeOffset VisitTime);