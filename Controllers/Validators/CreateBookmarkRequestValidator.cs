using bookmark_manager_app.Controllers.Requests;
using FluentValidation;

namespace bookmark_manager_app.Controllers.Validators;

public class CreateBookmarkRequestValidator : DefaultRequestValidator<CreateBookmarkRequest>
{
    public CreateBookmarkRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(200);
        RuleFor(x => x.Url).NotEmpty().IsValidUrl().MaximumLength(2048);
        RuleFor(x => x.Description).MaximumLength(1024);
        RuleFor(x => x.Tags).Must(x => x.Distinct().Count() == x.Length).WithMessage("Tags must be unique.");
        RuleFor(x => x.Tags)
            .Must(x => x.Length <= 20).WithMessage("A bookmark can have at most 20 unique tags.");
        RuleForEach(x => x.Tags).NotEmpty().MinimumLength(2).MaximumLength(25);
    }
}