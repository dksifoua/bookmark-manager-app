namespace bookmark_manager_app.Controllers.Responses;

public record CreateBookmarkResponse(string Title, string Url, string Description, IEnumerable<string> Tags);