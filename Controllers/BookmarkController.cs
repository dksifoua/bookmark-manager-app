using bookmark_manager_app.Controllers.Requests;
using bookmark_manager_app.Controllers.Responses;
using bookmark_manager_app.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Controllers;

[ApiController]
[Authorize]
[Route("/api/bookmarks")]
public class BookmarkController(
    BookmarkService bookmarkService,
    IValidator<CreateBookmarkRequest> createBookmarkRequestValidator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateBookmarkResponse>> CreateAsync(CreateBookmarkRequest request)
    {
        await createBookmarkRequestValidator.ValidateAndThrowAsync(request);

        var bookmark = await bookmarkService.CreateAsync(request.ToCommand());
        var tagNames = bookmark.Tags
            .Select(x => x.Name)
            .ToArray();

        return CreatedAtRoute(nameof(GetByIdAsync), new { Id = bookmark.BookmarkId },
            new CreateBookmarkResponse(bookmark.Title, bookmark.Url, bookmark.Description, tagNames));
    }

    [HttpGet("{id:long}", Name = nameof(GetByIdAsync))]
    public async Task<ActionResult<GetBookmarkResponse>> GetByIdAsync(long id)
    {
        var bookmark = await bookmarkService.GetByIdAsync(id);
        return Ok(GetBookmarkResponse.FromModel(bookmark));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetBookmarkResponse>>> GetAllByUserIdAsync()
    {
        var bookmarks = await bookmarkService.GetAllByUserIdAsync();
        return Ok(bookmarks.Select(GetBookmarkResponse.FromModel));
    }

    [HttpPatch("{id:long}/pin")]
    public async Task<IActionResult> TogglePinAsync(long id)
    {
        await bookmarkService.TogglePinAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:long}/archive")]
    public async Task<IActionResult> ToggleArchiveAsync(long id)
    {
        await bookmarkService.ToggleArchiveAsync(id);
        return NoContent();
    }
    
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        await bookmarkService.DeleteAsync(id);
        return NoContent();
    }
}