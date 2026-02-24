using BookmarkManagerApp.Controllers.Requests;
using FluentValidation;

namespace BookmarkManagerApp.Controllers.Validators;

public class CreateOrUpdateBookmarkRequestValidator : DefaultRequestValidator<CreateOrUpdateBookmarkRequest>
{
    public CreateOrUpdateBookmarkRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(200);
        RuleFor(x => x.Url).IsValidUrl().MaximumLength(2048);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1024);
        RuleFor(x => x.Tags).Must(x => x.Distinct().Count() == x.Length).WithMessage("Tags must be unique.");
        RuleFor(x => x.Tags)
            .Must(x => x.Length is >= 1 and <= 20).WithMessage("A bookmark can have at least one tag and at most 20 unique tags.");
        RuleForEach(x => x.Tags).NotEmpty().MinimumLength(2).MaximumLength(25);
    }
}