namespace BookmarkManagerApp.Controllers.Requests;

public record CreateVisitRequest(long BookmarkId, DateTimeOffset VisitTime);