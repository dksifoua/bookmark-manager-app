using bookmark_manager_app.Services;

namespace bookmark_manager_app.Controllers.Requests;

public record CreateBookmarkRequest(string Title, string Url, string Description, string[] Tags)
{
    public CreateBookmarkCommand ToCommand() => new CreateBookmarkCommand(Title, Url, Description, Tags);
}