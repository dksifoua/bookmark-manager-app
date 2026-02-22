using bookmark_manager_app.Services;

namespace bookmark_manager_app.Controllers.Requests;

public record CreateOrUpdateBookmarkRequest(string Title, string Url, string Description, string[] Tags)
{
    public CreateOrUpdateBookmarkCommand ToCommand() => new CreateOrUpdateBookmarkCommand(Title, Url, Description, Tags);
}