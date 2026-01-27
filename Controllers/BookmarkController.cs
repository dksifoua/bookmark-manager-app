using System.Security.Claims;
using BookmarkManagerApp.Controllers.Requests;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Controllers;

[ApiController]
[Authorize]
[Route("/api/bookmarks")]
public class BookmarkController(BookmarkService bookmarkService, IValidator<CreateBookmarkRequest> createBookmarkRequestValidator) : ControllerBase
{
    
    [HttpPost]
    public async Task<ActionResult<Bookmark>> CreateAsync(CreateBookmarkRequest request)
    {
        await createBookmarkRequestValidator.ValidateAndThrowAsync(request);
        
        var bookmark = await bookmarkService.CreateBookmarkAsync(request.ToCommand());
        return CreatedAtRoute(nameof(RetrieveByIdAsync), new { id = bookmark.BookmarkId }, bookmark);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bookmark>>> RetrieveAllAsync()
    {
        var bookmarks = await bookmarkService.RetrieveAllBookmarksByUserIdAsync();
        return Ok(bookmarks);
    }
    
    [HttpGet("{id:long}", Name = nameof(RetrieveByIdAsync))]
    public async Task<ActionResult<Bookmark>> RetrieveByIdAsync(long id)
    {
        return Ok(await bookmarkService.RetrieveBookmarkByIdAsync(id));
    }
    
    [HttpDelete("{id:long}")]
    public async Task DeleteAsync(long id)
    {
        await bookmarkService.DeleteBookmarkAsync(id);
        Ok();
    }
}