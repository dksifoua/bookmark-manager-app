namespace BookmarkManagerApp.Services.Commands;

public record CreateBookmarkCommand(string Title, string Url, string Description);