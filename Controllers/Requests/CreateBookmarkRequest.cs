using BookmarkManagerApp.Services.Commands;

namespace BookmarkManagerApp.Controllers.Requests;

public record CreateBookmarkRequest(string Title, string Url, string Description, IEnumerable<string>? Tagnames)
{
    public CreateBookmarkCommand ToCommand() => new(Title, Url, Description, Tagnames);
}