using BookmarkManagerApp.Repositories;
using BookmarkManagerApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Controllers;

[ApiController]
[Authorize]
[Route("/api/bookmark-tags")]
public class BookmarkTagController(BookmarkTagService bookmarkTagService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookmarkTagCount>>> RetrieveAllCountAsync() => 
        Ok(await bookmarkTagService.RetrieveAllBookmarkTagCountAsync());
}