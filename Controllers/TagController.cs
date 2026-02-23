using bookmark_manager_app.Controllers.Responses;
using bookmark_manager_app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Controllers;

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