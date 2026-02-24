using BookmarkManagerApp.Controllers.Requests;
using BookmarkManagerApp.Controllers.Responses;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Controllers;

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
        return CreatedAtRoute(nameof(GetVisitByIdAsync), new { Id = visit.VisitId }, VisitResponse.FromModel(visit));
    }
}