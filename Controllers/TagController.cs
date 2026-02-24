using BookmarkManagerApp.Controllers.Responses;
using BookmarkManagerApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Controllers;

[ApiController]
[Authorize]
[Route("/api/tags")]
public class TagController(TagService tagService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagResponse>>> RetrieveAllAsync()
    {
        var tags = await tagService.GetTagsAsync();
        return Ok(tags.Select(TagResponse.FromModel));
    }
}