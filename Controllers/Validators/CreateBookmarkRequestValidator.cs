using BookmarkManagerApp.Controllers.Requests;
using FluentValidation;

namespace BookmarkManagerApp.Controllers.Validators;

public class CreateBookmarkRequestValidator : DefaultRequestValidator<CreateBookmarkRequest>
{
    public CreateBookmarkRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Url).NotEmpty().IsValidUrl().MaximumLength(2048);
        RuleFor(x => x.Description).MaximumLength(1024);
        RuleFor(x => x.Tags)
            .Must(x => x is null || x.Count() <= 20).WithMessage("A bookmark can have at most 20 tags.");
        RuleForEach(x => x.Tags).NotEmpty().MaximumLength(25);
    }
}