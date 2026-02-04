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
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateBookmarkResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetBookmarkResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<GetBookmarkResponse>> GetByIdAsync(long id)
    {
        var bookmark = await bookmarkService.GetByIdAsync(id);
        return Ok(GetBookmarkResponse.FromModel(bookmark));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetBookmarkResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IEnumerable<GetBookmarkResponse>>> GetAllByUserIdAsync()
    {
        var bookmarks = await bookmarkService.GetAllByUserIdAsync();
        return Ok(bookmarks.Select(GetBookmarkResponse.FromModel));
    }

    [HttpPatch("{id:long}/pin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> TogglePinAsync(long id)
    {
        await bookmarkService.TogglePinAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:long}/archive")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ToggleArchiveAsync(long id)
    {
        await bookmarkService.ToggleArchiveAsync(id);
        return NoContent();
    }
    
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        await bookmarkService.DeleteAsync(id);
        return NoContent();
    }
}