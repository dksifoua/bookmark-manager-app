namespace BookmarkManagerApp.Controllers.Responses;

public record CreateBookmarkResponse(string Title, string Url, string Description, IEnumerable<string> Tagnames);