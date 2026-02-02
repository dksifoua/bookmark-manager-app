using bookmark_manager_app.Controllers.Requests;
using FluentValidation;

namespace bookmark_manager_app.Controllers.Validators;

public class CreateVisitRequestValidator : DefaultRequestValidator<CreateVisitRequest>
{
    public CreateVisitRequestValidator()
    {
        RuleFor(x => x.BookmarkId).GreaterThan(0);
        RuleFor(x => x.VisitTime).GreaterThan(DateTimeOffset.UtcNow.AddYears(-1));
    }
}