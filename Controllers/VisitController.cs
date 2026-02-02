using bookmark_manager_app.Controllers.Requests;
using bookmark_manager_app.Controllers.Responses;
using bookmark_manager_app.Models;
using bookmark_manager_app.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Controllers;

[ApiController]
[Authorize]
[Route("/api/visits")]
public class VisitController(VisitService visitService, IValidator<CreateVisitRequest> createVisitRequestValidator) : ControllerBase
{
    [HttpGet("{id:long}", Name = nameof(GetVisitByIdAsync))]
    public async Task<ActionResult<VisitResponse>> GetVisitByIdAsync(long id)
    {
        var visit = await visitService.GetByIdAsync(id);
        return Ok(new VisitResponse(visit.BookmarkId, visit.VisitTime));
    }
    
    [HttpPost]
    public async Task<ActionResult<VisitResponse>> CreateAsync(CreateVisitRequest request)
    {
        await createVisitRequestValidator.ValidateAndThrowAsync(request);
        
        var visit = await visitService.CreateAsync(new Visit{ BookmarkId = request.BookmarkId, VisitTime = request.VisitTime});
        return CreatedAtRoute(nameof(GetVisitByIdAsync), new { Id = visit.VisitId }, new VisitResponse(visit.BookmarkId, visit.VisitTime));
    }
}