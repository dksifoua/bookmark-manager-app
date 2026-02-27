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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TagResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IEnumerable<TagResponse>>> RetrieveAllAsync()
    {
        var tags = await tagService.GetTagsAsync();
        return Ok(tags.Select(TagResponse.FromModel));
    }
}