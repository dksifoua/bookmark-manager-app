using bookmark_manager_app.Repositories;
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
    public async Task<ActionResult<IEnumerable<TagCount>>> RetrieveAllAsync() => 
        Ok(await tagService.GetTagsByUserIdAsync());
}