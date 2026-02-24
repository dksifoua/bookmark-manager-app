using BookmarkManagerApp.Services;

namespace BookmarkManagerApp.Controllers.Requests;

public record CreateOrUpdateBookmarkRequest(string Title, string Url, string Description, string[] Tags)
{
    public CreateOrUpdateBookmarkCommand ToCommand() => new CreateOrUpdateBookmarkCommand(Title, Url, Description, Tags);
}