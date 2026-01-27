using System.Security.Claims;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Controllers;

[ApiController]
[Authorize]
[Route("/api/bookmarks")]
public class BookmarkController(BookmarkService bookmarkService, ClaimsPrincipal claimsPrincipal) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bookmark>>> GetAllBookmarks()
    {
        var bookmarks = await bookmarkService.GetAllBookmarksAsync();
        return Ok(bookmarks);
    }
}